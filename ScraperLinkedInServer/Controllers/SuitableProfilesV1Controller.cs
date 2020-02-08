using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Services.CompanyService.Interfaces;
using ScraperLinkedInServer.Services.SuitableProfileService.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/v1/suitable-profile")]
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

            return JsonSuccess(response);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IHttpActionResult> GetSuitableProfilesAsync(DateTime startDate, DateTime endDate, int accountId, int page, int size)
        {
            var response = new SuitableProfilesResponse();

            startDate = startDate == DateTime.MinValue ? DateTime.UtcNow.AddDays(-7) : startDate.ToUniversalTime();
            endDate = endDate == DateTime.MinValue ? DateTime.UtcNow : endDate.ToUniversalTime();

            var suitableProfilesVM = await suitableProfileService.GetSuitableProfilesAsync(startDate, endDate, accountId, page, size);
            response.SuitableProfilesViewModel = suitableProfilesVM;
            response.CountCompaniesInProcess = await companyService.GetCountCompaniesInProcess(accountId);

            return JsonSuccess(response);
        }

        [HttpPost]
        [Route("windows-service-worker")]
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> InsertSuitableProfilesAsync(SuitableProfilesRequest request)
        {
            var response = new SuitableProfilesResponse();

            await suitableProfileService.InsertSuitableProfilesAsync(request.SuitableProfilesViewModel);

            return JsonSuccess(response);
        }
    }
}
