using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Response
{
    public class SuitableProfileResponse
    {
        public SuitableProfileViewModel SuitableProfileViewModel { get; set; }
    }

    public class SuitableProfilesResponse
    {
        public IEnumerable<SuitableProfileViewModel> SuitableProfilesViewModel { get; set; }

        public int CountCompaniesInProcess { get; set; }
    }
}