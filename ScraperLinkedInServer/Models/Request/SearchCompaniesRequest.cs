using ScraperLinkedInServer.Models.Types;
using System;

namespace ScraperLinkedInServer.Models.Request
{
    public class SearchCompaniesRequest
    {
        public string SearchValue { get; set; }
        public SortedCompaniesFieldTypes SortedFieldType { get; set; }
        public bool IsAscending { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}