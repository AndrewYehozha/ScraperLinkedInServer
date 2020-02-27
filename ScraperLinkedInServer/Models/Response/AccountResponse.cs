using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Response
{
    public class AccountResponse : BaseResponse
    {
        public AccountViewModel AccountViewModel { get; set; }
    }

    public class AccountsIdsResponse : BaseResponse
    {
        public IEnumerable<int> Ids { get; set; }
    }
}