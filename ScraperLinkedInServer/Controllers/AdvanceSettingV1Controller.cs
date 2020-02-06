using ScraperLinkedInServer.Services.AdvanceSettingService.Interfaces;
using ScraperLinkedInServer.Models.Request;
using System.Threading.Tasks;
using System.Web.Http;
using ScraperLinkedInServer.Models.Response;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/v1/advance-settings")]
    public class AdvanceSettingV1Controller : ScraperLinkedInApiController
    {
        IAdvanceSettingService advanceSettingService;

        public AdvanceSettingV1Controller(
            IAdvanceSettingService advanceSettingService)
        {
            this.advanceSettingService = advanceSettingService;
        }

        [HttpGet]
        [Route("{accountId}")]
        [Authorize]
        public async Task<IHttpActionResult> GetAdvanceSettingByAccountId(int accountId)
        {
            var response = new AdvanceSettingResponse();

            var advanceSettingVM = await advanceSettingService.GetAdvanceSettingByAccountId(accountId);
            response.AdvanceSettingViewModel = advanceSettingVM;

            return JsonSuccess(response);
        }

        [HttpPut]
        [Route("advance-setting-management")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateAdvanceSettingAsync(AdvanceSettingRequest request)
        {
            var response = new AdvanceSettingResponse();

            await advanceSettingService.UpdateAdvanceSettingAsync(request.AdvanceSettingViewModel);

            return JsonSuccess(response);
        }
    }
}
