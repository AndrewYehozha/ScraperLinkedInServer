using ScraperLinkedInServer.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.DebugLogRepository.Interfaces
{
    public interface IDebugLogRepository
    {
        Task<IEnumerable<DebugLog>> GetDebugLogsAsync(int accountId, int batchSize);

        Task<IEnumerable<DebugLog>> GetNewDebugLogsAsync(int accountId, int lastDebugLogId, int size);

        Task InsertDebugLogAsync(DebugLog debugLog);

        Task InsertDebugLogsAsync(IEnumerable<DebugLog> debugLogs);
    }
}
