using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Response
{
    public class ProfileResponse : BaseResponse
    {
        public ProfileViewModel ProfileViewModel { get; set; }
    }

    public class ProfilesResponse : BaseResponse
    {
        public IEnumerable<ProfileViewModel> ProfilesViewModel { get; set; }

        public int CountProfilesInProcess { get; set; }

        public int CountNewProfiles { get; set; }
    }
}