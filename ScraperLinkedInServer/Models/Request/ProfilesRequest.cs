using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Request
{
    public class ProfileRequest
    {
        public ProfileViewModel ProfileViewModel { get; set; }
    }

    public class ProfilesRequest
    {
        public IEnumerable<ProfileViewModel> ProfilesViewModel { get; set; }
    }
}