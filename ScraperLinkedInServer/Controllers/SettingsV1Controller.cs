using ScraperLinkedInServer.Extensions;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Services.SettingService.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/v1/settings")]
    public class SettingsV1Controller : ScraperLinkedInApiController
    {
        private readonly ISettingService settingService;

        public SettingsV1Controller(
            ISettingService settingService)
        {
            this.settingService = settingService;
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IHttpActionResult> GetSettingAsync()
        {
            var response = new SettingResponse();

            var accountId = Identity.ToAccountID();
            response.SettingViewModel = await settingService.GetSettingByAccountIdAsync(accountId);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPut]
        [Route("setting-management")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateSettingAsync(SettingRequest request)
        {
            var response = new AdvanceSettingResponse();

            var accountId = Identity.ToAccountID();
            if (request.SettingViewModel.AccountId != accountId)
            {
                response.ErrorMessage = "Not permissions";
                response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
            else
            {
                await settingService.UpdateSettingAsync(request.SettingViewModel);
                response.StatusCode = (int)HttpStatusCode.OK;
            }

            return Ok(response);
        }
    }
}
