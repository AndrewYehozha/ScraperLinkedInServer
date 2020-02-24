using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Response
{
    public class DebugLogsResponse : BaseResponse
    {
        public IEnumerable<DebugLogViewModel> DebugLogsViewModel { get; set; }
    }
}