using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.DebugLogService.Interfaces
{
    public interface IDebugLogService
    {
        Task<IEnumerable<DebugLogViewModel>> GetDebugLogsAsync(int accountId, int batchSize);

        Task<IEnumerable<DebugLogViewModel>> GetNewDebugLogsAsync(int accountId, int lastDebugLogId, int size);

        Task InsertDebugLogAsync(DebugLogViewModel debugLogVM);

        Task InsertDebugLogsAsync(IEnumerable<DebugLogViewModel> debugLogsVM);
    }
}
