using ScraperLinkedInServer.Models.Request;

namespace ScraperLinkedInServer.Models.Response
{
    public class ProfileResponse : ProfileRequest
    {

    }

    public class ProfilesResponse : ProfilesRequest
    {
        public int CountProfilesInProcess { get; set; }

        public int CountNewProfiles { get; set; }
    }
}