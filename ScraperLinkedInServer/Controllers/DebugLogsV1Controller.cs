using ScraperLinkedInServer.Extensions;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Services.DebugLogService.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/v1/debug-logs")]
    public class DebugLogsV1Controller : ScraperLinkedInApiController
    {
        private readonly IDebugLogService _debugLogService;

        public DebugLogsV1Controller(
            IDebugLogService debugLogService)
        {
            _debugLogService = debugLogService;
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IHttpActionResult> GetDebugLogsAsync(int batchSize)
        {
            var response = new DebugLogsResponse();

            var accountId = Identity.ToAccountID();
            var debugLogsVM = await _debugLogService.GetDebugLogsAsync(accountId, batchSize);
            response.DebugLogsViewModel = debugLogsVM;
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IHttpActionResult> GetNewDebugLogsAsync(int lastDebugLogId, int size)
        {
            var response = new DebugLogsResponse();

            var accountId = Identity.ToAccountID();
            var debugLogsVM = await _debugLogService.GetNewDebugLogsAsync(accountId, lastDebugLogId, size);
            response.DebugLogsViewModel = debugLogsVM;
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPost]
        [Route("log")]
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> InsertDebugLogAsync(DebugLogRequest request)
        {
            var response = new DebugLogsResponse();

            var accountId = Identity.ToAccountID();
            request.DebugLogViewModel.AccountId = accountId;
            await _debugLogService.InsertDebugLogAsync(request.DebugLogViewModel);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> InsertDebugLogsAsync(DebugLogsRequest request)
        {
            var response = new DebugLogsResponse();

            var accountId = Identity.ToAccountID();
            foreach (var debugLog in request.DebugLogsViewModel)
            {
                debugLog.AccountId = accountId;
            }
            await _debugLogService.InsertDebugLogsAsync(request.DebugLogsViewModel);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }
    }
}
