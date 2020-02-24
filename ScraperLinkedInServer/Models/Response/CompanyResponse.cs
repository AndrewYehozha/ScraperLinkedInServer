using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Response
{
    public class CompanyResponse : BaseResponse
    {
        public CompanyViewModel CompanyViewModel { get; set; }
    }

    public class CompaniesResponse : BaseResponse
    {
        public IEnumerable<CompanyViewModel> CompaniesViewModel { get; set; }

        public int CountCompaniesInProcess { get; set; }
    }
}