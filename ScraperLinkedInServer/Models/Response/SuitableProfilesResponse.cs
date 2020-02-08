using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Request;

namespace ScraperLinkedInServer.Models.Response
{
    public class SuitableProfileResponse
    {
        public SuitableProfileViewModel SuitableProfileViewModel { get; set; }
    }

    public class SuitableProfilesResponse : SuitableProfilesRequest
    {
        public int CountCompaniesInProcess { get; set; }
    }
}