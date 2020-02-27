using ScraperLinkedInServer.Extensions;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Services.SettingService.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/v1/settings")]
    public class SettingsV1Controller : ScraperLinkedInApiController
    {
        private readonly ISettingService _settingService;

        public SettingsV1Controller(
            ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpGet]
        [Route("setting")]
        [Authorize]
        public async Task<IHttpActionResult> GetSettingAsync()
        {
            var response = new SettingsResponse();

            var accountId = Identity.ToAccountID();
            response.SettingsViewModel = await _settingService.GetSettingByAccountIdAsync(accountId);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpGet]
        [Route("setting/{accountId}")]
        [Authorize]
        public async Task<IHttpActionResult> GetSettingByAccountIdAsync(int accountId)
        {
            var response = new SettingsResponse();

            response.SettingsViewModel = await _settingService.GetSettingByAccountIdAsync(accountId);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPut]
        [Route("setting")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateSettingAsync(SettingsRequest request)
        {
            var response = new SettingsResponse();

            var accountId = Identity.ToAccountID();
            if (request.SettingViewModel.AccountId != accountId)
            {
                response.ErrorMessage = "Not permissions";
                response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
            else
            {
                await _settingService.UpdateSettingAsync(request.SettingViewModel);
                response.StatusCode = (int)HttpStatusCode.OK;
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("setting/scraper-status")]
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> UpdateScraperStatus(UpdateScraperStatusRequest request)
        {
            var response = new BaseResponse();

            var accountId = Identity.ToAccountID();
            await _settingService.UpdateScraperStatus(accountId, request.Status);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }
    }
}
