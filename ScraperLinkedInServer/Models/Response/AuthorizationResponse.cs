using ScraperLinkedInServer.Database;

namespace ScraperLinkedInServer.Models.Response
{
    public class AuthorizationResponse
    {
        public Account Account { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}