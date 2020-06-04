namespace ScraperLinkedInServer.Models.Response
{
    public class GenerateTokenResponse
    {
        public string Token { get; set; }
        public System.DateTime Expires { get; set; }
    }
}