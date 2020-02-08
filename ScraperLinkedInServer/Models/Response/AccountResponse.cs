using ScraperLinkedInServer.Models.Request;

namespace ScraperLinkedInServer.Models.Response
{
    public class AccountResponse : AccountRequest
    {
        public string Message { get; set; }
    }
}