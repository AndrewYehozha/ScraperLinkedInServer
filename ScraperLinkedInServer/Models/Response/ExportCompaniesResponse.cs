using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Response
{
    public class ExportCompaniesResponse
    {
        public IEnumerable<ExportCompaniesViewModel> ExportCompaniesViewModel { get; set; }
        public int CompaniesEntriesCount { get; set; }
    }
}