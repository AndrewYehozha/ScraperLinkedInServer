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
        private readonly ISuitableProfileService suitableProfileService;
        private readonly ICompanyService companyService;

        public SuitableProfilesV1Controller(
            ISuitableProfileService suitableProfileService,
            ICompanyService companyService)
        {
            this.suitableProfileService = suitableProfileService;
            this.companyService = companyService;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<IHttpActionResult> GetSuitableProfileAsync(int id)
        {
            var response = new SuitableProfileResponse();

            var suitableProfileVM = await suitableProfileService.GetSuitableProfileByIdAsync(id);
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
            var suitableProfilesVM = await suitableProfileService.GetSuitableProfilesAsync(startDate, endDate, accountId, page, size);
            response.SuitableProfilesViewModel = suitableProfilesVM;
            response.CountCompaniesInProcess = await companyService.GetCountCompaniesInProcess(accountId);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPost]
        [Route("windows-service-worker")]
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> InsertSuitableProfilesAsync(SuitableProfilesRequest request)
        {
            var response = new SuitableProfilesResponse();

            var accountId = Identity.ToAccountID();
            foreach (var suitableProfile in request.SuitableProfilesViewModel)
            {
                suitableProfile.AccountID = accountId;
            }

            await suitableProfileService.InsertSuitableProfilesAsync(request.SuitableProfilesViewModel);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }
    }
}
