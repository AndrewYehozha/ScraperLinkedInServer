using ScraperLinkedInServer.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.DebugLogRepository.Interfaces
{
    public interface IDebugLogRepository
    {
        Task<List<DebugLog>> GetNewDebugLogsAsync(int accountId, int lastDebugLogId, int size);

        Task<IEnumerable<DebugLog>> GetLogsAsync(int accountId, int batchSize);

        Task InsertMessagesAsync(List<DebugLog> debugLogs);

        Task InsertMessageAsync(DebugLog debugLog);
    }
}
