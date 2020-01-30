using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Response
{
    public class ProfilesResponse
    {
        IEnumerable<ProfileViewModel> ProfilesViewModel { get; set; }
    }
}