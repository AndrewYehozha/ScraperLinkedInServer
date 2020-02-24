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
        private readonly IAdvanceSettingService _advanceSettingService;

        public AdvanceSettingsV1Controller(
            IAdvanceSettingService advanceSettingService)
        {
            _advanceSettingService = advanceSettingService;
        }

        [HttpGet]
        [Route("setting")]
        [Authorize]
        public async Task<IHttpActionResult> GetAdvanceSettingByAccountId()
        {
            var response = new AdvanceSettingsResponse();

            var accountId = Identity.ToAccountID();
            var advanceSettingVM = await _advanceSettingService.GetAdvanceSettingByAccountId(accountId);
            response.AdvanceSettingsViewModel = advanceSettingVM;
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPut]
        [Route("setting")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateAdvanceSettingAsync(AdvanceSettingsRequest request)
        {
            var response = new AdvanceSettingsResponse();

            var accountId = Identity.ToAccountID();
            if (request.AdvanceSettingViewModel.AccountId != accountId)
            {
                response.ErrorMessage = "Not permissions";
                response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
            else
            {
                await _advanceSettingService.UpdateAdvanceSettingAsync(request.AdvanceSettingViewModel);
                response.StatusCode = (int)HttpStatusCode.OK;
            }

            return Ok(response);
        }
    }
}
