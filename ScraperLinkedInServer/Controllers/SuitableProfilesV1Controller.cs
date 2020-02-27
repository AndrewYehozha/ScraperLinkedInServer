using ScraperLinkedInServer.Extensions;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Services.CompanyService.Interfaces;
using ScraperLinkedInServer.Services.SuitableProfileService.Interfaces;
using System;
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
        [Route("suitable-profile/{id}")]
        [Authorize]
        public async Task<IHttpActionResult> GetSuitableProfileAsync(int id)
        {
            var response = new SuitableProfileResponse();

            var suitableProfileVM = await _suitableProfileService.GetSuitableProfileByIdAsync(id);
            response.SuitableProfileViewModel = suitableProfileVM;
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IHttpActionResult> GetSuitableProfilesAsync(DateTime startDate, DateTime endDate, int page, int size)
        {
            var response = new SuitableProfilesResponse();

            startDate = startDate == DateTime.MinValue ? DateTime.UtcNow.AddDays(-7) : startDate.ToUniversalTime();
            endDate = endDate == DateTime.MinValue ? DateTime.UtcNow : endDate.ToUniversalTime();

            var accountId = Identity.ToAccountID();
            var suitableProfilesVM = await _suitableProfileService.GetSuitableProfilesAsync(startDate, endDate, accountId, page, size);
            response.SuitableProfilesViewModel = suitableProfilesVM;
            response.CountCompaniesInProcess = await _companyService.GetCountCompaniesInProcess(accountId);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
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
