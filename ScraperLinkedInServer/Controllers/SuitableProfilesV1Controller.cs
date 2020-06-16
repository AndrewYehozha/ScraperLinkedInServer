using CsvHelper;
using ScraperLinkedInServer.Extensions;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Services.CompanyService.Interfaces;
using ScraperLinkedInServer.Services.SuitableProfileService.Interfaces;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/v1/suitable-profiles")]
    public class SuitableProfilesV1Controller : ScraperLinkedInApiController
    {
        private readonly ISuitableProfileService _suitableProfileService;
        private readonly ICompanyService _companyService;

        public SuitableProfilesV1Controller(
            ISuitableProfileService suitableProfileService,
            ICompanyService companyService)
        {
            _suitableProfileService = suitableProfileService;
            _companyService = companyService;
        }

        [HttpGet]
        [Route("suitable-profile")]
        [Authorize]
        public async Task<IHttpActionResult> GetSuitableProfileAsync(int id)
        {
            var response = new SuitableProfileResponse();

            var suitableProfileVM = await _suitableProfileService.GetSuitableProfileByIdAsync(id);
            response.SuitableProfileViewModel = suitableProfileVM;
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPost]
        [Route("search")]
        [Authorize]
        public async Task<IHttpActionResult> GetSuitableProfilesAsync(SearchSuitablesProfilesRequest request)
        {
            var accountId = Identity.ToAccountID();

            var response = await _suitableProfileService.GetSuitableProfilesAsync(accountId, request);

            return Ok(response);
        }


        [HttpPost]
        [Route("export")]
        [Authorize]
        public async Task<IHttpActionResult> ExportSuitableProfilesAsync(SearchSuitablesProfilesRequest request)
        {
            var result = new ExportFileResponse();
            var accountId = Identity.ToAccountID();

            var suitablesProfilesList = await _suitableProfileService.SearchExportSuitablesProfilesAsync(accountId, request);
            if (suitablesProfilesList.SuitablesProfilesEntriesCount > 0)
            {
                try
                {
                    using (var stream = new MemoryStream())
                    using (var reader = new StreamReader(stream))
                    using (var writer = new StreamWriter(stream))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(suitablesProfilesList.ExportSuitableProfilesViewModel);
                        writer.Flush();
                        stream.Position = 0;
                        result.Content = reader.ReadToEnd();
                        reader.Close();
                        result.ContentType = "application/vnd.ms-excel";
                        result.ContentEntriesCount = suitablesProfilesList.SuitablesProfilesEntriesCount;
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
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> InsertSuitableProfilesAsync(SuitableProfilesRequest request)
        {
            var response = new BaseResponse();

            await _suitableProfileService.InsertSuitableProfilesAsync(request.SuitableProfilesViewModel);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }
    }
}
