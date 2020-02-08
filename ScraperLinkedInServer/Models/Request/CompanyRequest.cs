using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Request
{
    public class CompanyRequest
    {
        public CompanyViewModel CompanyViewModel { get; set; }
    }

    public class CompaniesRequest
    {
        public IEnumerable<CompanyViewModel> CompaniesViewModel { get; set; }
    }
}