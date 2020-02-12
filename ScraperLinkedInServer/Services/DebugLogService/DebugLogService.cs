using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.ObjectMappers;
using ScraperLinkedInServer.Repositories.DebugLogRepository.Interfaces;
using ScraperLinkedInServer.Services.DebugLogService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.DebugLogService
{
    public class DebugLogService : IDebugLogService
    {
        private readonly IDebugLogRepository _debugLogRepository;

        public DebugLogService(
            IDebugLogRepository debugLogRepository)
        {
            _debugLogRepository = debugLogRepository;
        }

        public async Task<IEnumerable<DebugLogViewModel>> GetDebugLogsAsync(int accountId, int batchSize)
        {
            var debugLogsDB = await _debugLogRepository.GetDebugLogsAsync(accountId, batchSize);
            return Mapper.Instance.Map<IEnumerable<DebugLog>, IEnumerable<DebugLogViewModel>>(debugLogsDB);
        }

        public async Task<IEnumerable<DebugLogViewModel>> GetNewDebugLogsAsync(int accountId, int lastDebugLogId, int size)
        {
            var debugLogsDB = await _debugLogRepository.GetNewDebugLogsAsync(accountId, lastDebugLogId, size);
            return Mapper.Instance.Map<IEnumerable<DebugLog>, IEnumerable<DebugLogViewModel>>(debugLogsDB);
        }

        public async Task InsertDebugLogAsync(DebugLogViewModel debugLogVM)
        {
            var debugLogDB = Mapper.Instance.Map<DebugLogViewModel, DebugLog>(debugLogVM);
            await _debugLogRepository.InsertDebugLogAsync(debugLogDB);
        }

        public async Task InsertDebugLogsAsync(IEnumerable<DebugLogViewModel> debugLogsVM)
        {
            var debugLogsDB = Mapper.Instance.Map<IEnumerable<DebugLogViewModel>, IEnumerable<DebugLog>>(debugLogsVM);
            await _debugLogRepository.InsertDebugLogsAsync(debugLogsDB);
        }
    }
}