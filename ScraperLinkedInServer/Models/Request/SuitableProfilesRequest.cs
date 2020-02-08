using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Request
{
    public class SuitableProfilesRequest
    {
        public IEnumerable<SuitableProfileViewModel> SuitableProfilesViewModel { get; set; }
    }
}