using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Response
{
    public class SuitableProfilesResponse
    {
        IEnumerable<SuitableProfileViewModel> SuitableProfilesViewModel { get; set; }
    }
}