using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Response
{
    public class ExportSuitablesProfilesResponse
    {
        public IEnumerable<SearchSuitableProfilesViewModel> ExportSuitableProfilesViewModel { get; set; }
        public int SuitablesProfilesEntriesCount { get; set; }
    }
}