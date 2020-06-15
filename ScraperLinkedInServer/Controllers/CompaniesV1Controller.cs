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
using System.Net.Http.Headers;
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
            var response = new SearchCompaniesResponse();
            var accountId = Identity.ToAccountID();

            response = await _companyService.SearchCompaniesAsync(accountId, request);
            response.StatusCode = (int)HttpStatusCode.OK;

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
                            csv.Configuration.RegisterClassMap<CompanyCSVMap>();
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
        public async Task<HttpResponseMessage> ExportCompaniesAsync(SearchCompaniesRequest request)
        {
            //var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("Test"));
            var stream = new FileStream(@"D:\Важное\Учеба\IV-Курс\Практика\Основы с работы\companies-5-4-2020.csv", FileMode.Open);
            // processing the stream.
            var result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
            result.Content.Headers.ContentLength = stream.Length;
            return result;


            //var response = Request.CreateResponse(HttpStatusCode.OK);
            //var accountId = Identity.ToAccountID();
            //var companiesList = await _companyService.SearchCompaniesAsync(accountId, request);

            //if (companiesList.SearchCompaniesViewModel.Any())
            //{
            //    using (var stream = new MemoryStream())
            //    {
            //        using (var writer = new StreamWriter(stream))
            //        {
            //            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            //            {
            //                try
            //                {
            //                    csv.Configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.Trim;
            //                    csv.Configuration.RegisterClassMap<CompanyCSVMap>();
            //                    await csv.WriteRecordsAsync(companiesList.SearchCompaniesViewModel);
            //                    var c = stream.ToString();
            //                    response.StatusCode = HttpStatusCode.OK;
            //                    response.Content = new StringContent(stream.ToString());
            //                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            //                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment"); //attachment will force download
            //                    response.Content.Headers.ContentDisposition.FileName = $"companies_{ DateTime.UtcNow.ToString("MM-dd-yyyy-H-mm-ss") }.csv";
            //                }
            //                catch (Exception ex)
            //                {
            //                    response.ReasonPhrase = ex.Message;
            //                    response.StatusCode = HttpStatusCode.BadRequest;
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    response.ReasonPhrase = "The file is empty";
            //    response.StatusCode = HttpStatusCode.OK;
            //}
            ////return File(System.IO.File.ReadAllBytes(@"D:\Важное\Учеба\IV-Курс\Практика\Основы с работы\companies-5-4-2020 - Copy.csv"), "application/vnd.ms-excel", "companies-5-4-2020 - Copy.csv");
            //return response;
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
