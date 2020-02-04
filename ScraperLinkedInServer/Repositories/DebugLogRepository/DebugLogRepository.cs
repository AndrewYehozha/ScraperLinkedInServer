﻿using ScraperLinkedInServer.Database;
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
        public async Task<List<DebugLog>> GetNewDebugLogsAsync(int accountId, int lastDebugLogId, int size = 50)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.DebugLogs.Where(x => x.Id > lastDebugLogId && x.AccountId == accountId)
                                         .Take(size)
                                         .ToListAsync();
            }
        }

        public async Task<IEnumerable<DebugLog>> GetLogsAsync(int accountId, int batchSize = 50)
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

        public async Task InsertMessagesAsync(List<DebugLog> debugLogs)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                foreach (var debugLog in debugLogs)
                {
                    var log = new DebugLog
                    {
                        Remarks = !string.IsNullOrEmpty(debugLog.Remarks) ? debugLog.Remarks : "",
                        Logs = debugLog.Logs,
                        CreatedOn = DateTime.UtcNow
                    };

                    db.DebugLogs.Add(log);
                }

                await db.SaveChangesAsync();
            }
        }

        public async Task InsertMessageAsync(DebugLog debugLog)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var log = new DebugLog
                {
                    Remarks = !string.IsNullOrEmpty(debugLog.Remarks) ? debugLog.Remarks : "",
                    Logs = debugLog.Logs,
                    CreatedOn = DateTime.UtcNow
                };

                db.DebugLogs.Add(log);
                await db.SaveChangesAsync();
            }
        }
    }
}