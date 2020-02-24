using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Repositories.DebugLogRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.DebugLogRepository
{
    public class DebugLogRepository : IDebugLogRepository
    {
        public async Task<IEnumerable<DebugLog>> GetDebugLogsAsync(int accountId, int batchSize = 50)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.DebugLogs.Where(x => x.AccountId == accountId)
                                         .OrderByDescending(x => x.Id)
                                         .Take(batchSize)
                                         .OrderBy(x => x.Id)
                                         .ToListAsync();
            }
        }

        public async Task<IEnumerable<DebugLog>> GetNewDebugLogsAsync(int accountId, int lastDebugLogId, int size = 50)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.DebugLogs.Where(x => x.Id > lastDebugLogId && x.AccountId == accountId)
                                         .Take(size)
                                         .ToListAsync();
            }
        }

        public async Task InsertDebugLogAsync(DebugLog debugLog)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                db.DebugLogs.Add(debugLog);
                await db.SaveChangesAsync();
            }
        }

        public async Task InsertDebugLogsAsync(IEnumerable<DebugLog> debugLogs)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                foreach (var debugLog in debugLogs)
                {
                    db.DebugLogs.Add(debugLog);
                }

                await db.SaveChangesAsync();
            }
        }
    }
}