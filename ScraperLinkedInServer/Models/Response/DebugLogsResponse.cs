using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Response
{
    public class DebugLogsResponse
    {
        public IEnumerable<DebugLogViewModel> DebugLogsViewModel { get; set; }
    }
}