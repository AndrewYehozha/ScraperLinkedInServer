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

        internal int CountCompaniesInProcess { get; set; }
    }
}