using ScraperLinkedInServer.Models.Entities;

namespace ScraperLinkedInServer.Models.Response
{
    public class AuthorizationResponse
    {
        public AccountViewModel Account { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}