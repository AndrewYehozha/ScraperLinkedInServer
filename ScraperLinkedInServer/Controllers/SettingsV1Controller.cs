using ScraperLinkedInServer.Extensions;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Services.SettingService.Interfaces;
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
        public async Task<IHttpActionResult> GetSettingByAccountIdAsync()
        {
            var response = new SettingResponse();

            var accountId = Identity.ToAccountID();
            response.SettingViewModel = await settingService.GetSettingByAccountIdAsync(accountId);

            return JsonSuccess(response);
        }

        [HttpPut]
        [Route("setting-management")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateSettingAsync(SettingRequest request)
        {
            var response = new AdvanceSettingResponse();

            await settingService.UpdateSettingAsync(request.SettingViewModel);

            return JsonSuccess(response);
        }
    }
}
