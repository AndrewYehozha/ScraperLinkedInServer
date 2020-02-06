using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Request
{
    public class DebugLogRequest
    {
        public DebugLogViewModel DebugLogViewModel { get; set; }
    }

    public class DebugLogsRequest
    {
        public IEnumerable<DebugLogViewModel> DebugLogsViewModel { get; set; }
    }
}