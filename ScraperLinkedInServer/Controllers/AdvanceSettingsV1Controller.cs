using ScraperLinkedInServer.Services.AdvanceSettingService.Interfaces;
using ScraperLinkedInServer.Models.Request;
using System.Threading.Tasks;
using System.Web.Http;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Extensions;
using System.Net;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/v1/advance-settings")]
    public class AdvanceSettingsV1Controller : ScraperLinkedInApiController
    {
        private readonly IAdvanceSettingService advanceSettingService;

        public AdvanceSettingsV1Controller(
            IAdvanceSettingService advanceSettingService)
        {
            this.advanceSettingService = advanceSettingService;
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IHttpActionResult> GetAdvanceSettingByAccountId()
        {
            var response = new AdvanceSettingResponse();

            var accountId = Identity.ToAccountID();
            var advanceSettingVM = await advanceSettingService.GetAdvanceSettingByAccountId(accountId);
            response.AdvanceSettingViewModel = advanceSettingVM;
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPut]
        [Route("advance-setting-management")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateAdvanceSettingAsync(AdvanceSettingRequest request)
        {
            var response = new AdvanceSettingResponse();

            var accountId = Identity.ToAccountID();
            if (request.AdvanceSettingViewModel.AccountId != accountId)
            {
                response.ErrorMessage = "Not permissions";
                response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
            else
            {
                await advanceSettingService.UpdateAdvanceSettingAsync(request.AdvanceSettingViewModel);
                response.StatusCode = (int)HttpStatusCode.OK;
            }

            return Ok(response);
        }
    }
}
