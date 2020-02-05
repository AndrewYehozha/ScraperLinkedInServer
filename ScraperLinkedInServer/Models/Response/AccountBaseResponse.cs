using ScraperLinkedInServer.Models.Entities;

namespace ScraperLinkedInServer.Models.Response
{
    public class AccountBaseResponse
    {
        public AccountViewModel AccountViewModel { get; set; }
        public string Message { get; set; }
    }
}