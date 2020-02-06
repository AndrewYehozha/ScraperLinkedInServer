using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Services.DebugLogService.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/v1/debug-logs")]
    public class DebugLogV1Controller : ScraperLinkedInApiController
    {
        private readonly IDebugLogService debugLogService;

        public DebugLogV1Controller(
            IDebugLogService debugLogService)
        {
            this.debugLogService = debugLogService;
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IHttpActionResult> GetDebugLogsAsync(int accountId, int batchSize)
        {
            var response = new DebugLogsResponse();

            var debugLogsVM = await debugLogService.GetDebugLogsAsync(accountId, batchSize);
            response.DebugLogsViewModel = debugLogsVM;

            return JsonSuccess(response);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IHttpActionResult> GetNewDebugLogsAsync(int accountId, int lastDebugLogId, int size)
        {
            var response = new DebugLogsResponse();

            var debugLogsVM = await debugLogService.GetNewDebugLogsAsync(accountId, lastDebugLogId, size);
            response.DebugLogsViewModel = debugLogsVM;

            return JsonSuccess(response);
        }

        [HttpPost]
        [Route("debug-log-managment")]
        [Authorize]
        public async Task<IHttpActionResult> InsertDebugLogAsync(DebugLogRequest request)
        {
            var response = new DebugLogsResponse();

            await debugLogService.InsertDebugLogAsync(request.DebugLogViewModel);

            return JsonSuccess(response);
        }

        [HttpPost]
        [Route("debug-log-managment")]
        [Authorize]
        public async Task<IHttpActionResult> InsertDebugLogsAsync(DebugLogsRequest request)
        {
            var response = new DebugLogsResponse();

            await debugLogService.InsertDebugLogsAsync(request.DebugLogsViewModel);

            return JsonSuccess(response);
        }
    }
}
