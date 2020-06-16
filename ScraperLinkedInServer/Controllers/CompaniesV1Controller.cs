using CsvHelper;
using ScraperLinkedInServer.Extensions;
using ScraperLinkedInServer.Models.CSVMap;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Services.CompanyService.Interfaces;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/v1/companies")]
    public class CompaniesV1Controller : ScraperLinkedInApiController
    {
        private readonly ICompanyService _companyService;

        public CompaniesV1Controller(
            ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        [Route("company")]
        [Authorize]
        public async Task<IHttpActionResult> GetCompanyAsync(int id)
        {
            var response = new CompanyResponse();

            var accountId = Identity.ToAccountID();
            var company = await _companyService.GetCompanyByIdAsync(id, accountId);
            response.CompanyViewModel = company;
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPost]
        [Route("search")]
        [Authorize]
        public async Task<IHttpActionResult> SearchCompaniesAsync(SearchCompaniesRequest request)
        {
            var accountId = Identity.ToAccountID();

            var response = await _companyService.SearchCompaniesAsync(accountId, request);

            return Ok(response);
        }

        [HttpGet]
        [Route("for-search")] //windows-service-scraper
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> GetCompaniesForSearchAsync(int companyBatchSize)
        {
            var response = new CompaniesResponse();

            var accountId = Identity.ToAccountID();
            var companiesVM = await _companyService.GetCompaniesForSearchAsync(accountId, companyBatchSize);
            var countCompaniesInProcess = await _companyService.GetCountCompaniesInProcess(accountId);
            response.CompaniesViewModel = companiesVM;
            response.CountCompaniesInProcess = countCompaniesInProcess;
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpGet]
        [Route("for-search-suitable-profiles")]
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> GetCompaniesForSearchSuitableProfilesAsync(int accountId, int companyBatchSize)
        {
            var response = new CompaniesProfilesResponse();

            var companyProfilesVM = await _companyService.GetCompaniesProfilesForSearchAsync(accountId, companyBatchSize);
            response.CompanyProfilesViewModel = companyProfilesVM;
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpGet]
        [Route("count-in-process")]
        [Authorize]
        public async Task<IHttpActionResult> GetCountCompaniesInProcess()
        {
            var response = new CompaniesResponse();

            var accountId = Identity.ToAccountID();
            var countCompaniesInProcess = await _companyService.GetCountCompaniesInProcess(accountId);
            response.CountCompaniesInProcess = countCompaniesInProcess;
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPost]
        [Route("company")]
        [Authorize]
        public async Task<IHttpActionResult> InsertCompanyAsync(CompanyRequest request)
        {
            var response = new CompanyResponse();

            var accountId = Identity.ToAccountID();
            request.CompanyViewModel.AccountId = accountId;

            await _companyService.InsertCompanyAsync(request.CompanyViewModel);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPost]
        [Route("import")]
        [Authorize]
        public async Task<IHttpActionResult> ImportCompaniesAsync()
        {
            var response = new ImportCompaniesResponse();

            if (!Request.Content.IsMimeMultipartContent())
            {
                response.ErrorMessage = "This is not file";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Ok(response);
            }

            var importedCompaniesCount = 0;
            var accountId = Identity.ToAccountID();
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                var fileType = file.Headers.ContentDisposition.FileName.Trim('\"');
                if (!fileType.Equals("application/vnd.ms-excel"))
                {
                    response.ErrorMessage = "Invalid file type. Please, import the CSV file";
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Ok(response);
                }

                var fileContent = await file.ReadAsStringAsync();
                using (var reader = new StringReader(fileContent))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        try
                        {
                            csv.Configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.Trim;
                            csv.Configuration.RegisterClassMap<CompanyImportCSVMap>();
                            var companies = csv.GetRecords<CompanyViewModel>().ToList();
                            importedCompaniesCount += await _companyService.InsertCompaniesAsync(accountId, companies);
                        }
                        catch (Exception)
                        {
                            response.ErrorMessage = "The file does not have a single required column";
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return Ok(response);
                        }
                    }
                }
            }

            response.ImportedCompaniesCount = importedCompaniesCount;
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPost]
        [Route("export")]
        [Authorize]
        public async Task<IHttpActionResult> ExportCompaniesAsync(SearchCompaniesRequest request)
        {
            var result = new ExportFileResponse();
            var accountId = Identity.ToAccountID();

            var companiesList = await _companyService.SearchExportCompaniesAsync(accountId, request);
            if (companiesList.CompaniesEntriesCount > 0)
            {
                try
                {
                    using (var stream = new MemoryStream())
                    using (var reader = new StreamReader(stream))
                    using (var writer = new StreamWriter(stream))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.Configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.Trim;
                        csv.Configuration.RegisterClassMap<CompanyExportCSVMap>();
                        csv.WriteRecords(companiesList.ExportCompaniesViewModel);
                        writer.Flush();
                        stream.Position = 0;
                        result.Content = reader.ReadToEnd();
                        reader.Close();
                        result.ContentType = "application/vnd.ms-excel";
                        result.ContentEntriesCount = companiesList.CompaniesEntriesCount;
                        result.DateCreateUTC = $"{ DateTime.UtcNow.ToString("MM-dd-yy_H-mm-ss") }UTC";

                        result.StatusCode = (int)HttpStatusCode.OK;
                    }
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.Message;
                    result.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                result.ErrorMessage = "Entries with Company not found";
                result.StatusCode = (int)HttpStatusCode.NotFound;
            }

            return Ok(result);

            //var result = Request.CreateResponse(HttpStatusCode.OK);
            //if (companiesList.SearchCompaniesViewModel.Any())
            //{
            //        using (var writer = new StreamWriter("path\\to\\file.csv", false, System.Text.Encoding.UTF8))
            //        {
            //            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            //            {
            //                try
            //                {
            //                    csv.Configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.Trim;
            //                    csv.Configuration.RegisterClassMap<CompanyCSVMap>();
            //                    await csv.WriteRecordsAsync(companiesList.SearchCompaniesViewModel);


            //                    // processing the stream.
            //                    var fileStream = new FileStream(@"D:\Важное\Учеба\IV-Курс\Практика\Основы с работы\companies-5-4-2020.csv", FileMode.Open);
            //                    result.Content = new StreamContent(fileStream);
            //                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            //                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
            //                    result.Content.Headers.ContentLength = fileStream.Length;
            //                }
            //                catch (Exception ex)
            //                {
            //                    result.ReasonPhrase = ex.Message;
            //                    result.StatusCode = HttpStatusCode.BadRequest;
            //                }
            //            }
            //        }
            //}
            //else
            //{
            //    result.ReasonPhrase = "The file is empty";
            //    result.StatusCode = HttpStatusCode.OK;
            //}
            //return result;
        }

        [HttpPut]
        [Route("company")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateCompanyAsync(CompanyRequest request)
        {
            var response = new CompanyResponse();

            var accountId = Identity.ToAccountID();
            if (request.CompanyViewModel.AccountId != accountId)
            {
                response.ErrorMessage = "Not permissions";
                response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
            else
            {
                await _companyService.UpdateCompanyAsync(request.CompanyViewModel);
                response.StatusCode = (int)HttpStatusCode.OK;
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("")]
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> UpdateCompaniesAsync(CompaniesRequest request)
        {
            var response = new CompaniesResponse();

            await _companyService.UpdateCompaniesAsync(request.CompaniesViewModel);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPut]
        [Route("company/last-scraped-page")]
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> UpdateLastPageCompanyAsync(CompanyLastPageRequest request)
        {
            var response = new BaseResponse();

            var accountId = Identity.ToAccountID();
            await _companyService.UpdateLastPageCompanyAsync(accountId, request.CompanyId, request.LastScrapedPage);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }
    }
}
