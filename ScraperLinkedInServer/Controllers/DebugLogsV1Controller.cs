using ScraperLinkedInServer.Extensions;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Services.DebugLogService.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/v1/debug-logs")]
    public class DebugLogsV1Controller : ScraperLinkedInApiController
    {
        private readonly IDebugLogService debugLogService;

        public DebugLogsV1Controller(
            IDebugLogService debugLogService)
        {
            this.debugLogService = debugLogService;
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IHttpActionResult> GetDebugLogsAsync(int batchSize)
        {
            var response = new DebugLogsResponse();

            var accountId = Identity.ToAccountID();
            var debugLogsVM = await debugLogService.GetDebugLogsAsync(accountId, batchSize);
            response.DebugLogsViewModel = debugLogsVM;

            return JsonSuccess(response);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IHttpActionResult> GetNewDebugLogsAsync(int lastDebugLogId, int size)
        {
            var response = new DebugLogsResponse();

            var accountId = Identity.ToAccountID();
            var debugLogsVM = await debugLogService.GetNewDebugLogsAsync(accountId, lastDebugLogId, size);
            response.DebugLogsViewModel = debugLogsVM;

            return JsonSuccess(response);
        }

        [HttpPost]
        [Route("windows-service-scraper")]
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> InsertDebugLogAsync(DebugLogRequest request)
        {
            var response = new DebugLogsResponse();

            await debugLogService.InsertDebugLogAsync(request.DebugLogViewModel);

            return JsonSuccess(response);
        }

        [HttpPost]
        [Route("windows-service-scraper")]
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> InsertDebugLogsAsync(DebugLogsRequest request)
        {
            var response = new DebugLogsResponse();

            await debugLogService.InsertDebugLogsAsync(request.DebugLogsViewModel);

            return JsonSuccess(response);
        }
    }
}
