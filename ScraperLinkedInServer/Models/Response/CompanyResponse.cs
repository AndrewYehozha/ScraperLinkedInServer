using ScraperLinkedInServer.Models.Request;

namespace ScraperLinkedInServer.Models.Response
{
    public class CompanyResponse : CompanyRequest
    {
    }

    public class CompaniesResponse : CompaniesRequest
    {
        public int CountCompaniesInProcess { get; set; }
    }
}