using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Response
{
    public class SearchSuitableProfilesResponse : BaseResponse
    {
        public IEnumerable<SearchSuitableProfilesViewModel> SearchSuitableProfilesViewModel { get; set; }
        public int TotalCount { get; set; }
    }
}