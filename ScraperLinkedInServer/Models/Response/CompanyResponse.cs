using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Response
{
    public class CompanyResponse
    {
        public CompanyViewModel CompanyViewModel { get; set; }
    }

    public class CompaniesResponse
    {
        public IEnumerable<CompanyViewModel> CompaniesViewModel { get; set; }

        public int CountCompaniesInProcess { get; set; }
    }
}