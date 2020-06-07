using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Types;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Response
{
    public class SearchCompaniesResponse : BaseResponse
    {
        public IEnumerable<SearchCompaniesViewModel> SearchCompaniesViewModel { get; set; }
        public int TotalCount { get; set; }
    }
}