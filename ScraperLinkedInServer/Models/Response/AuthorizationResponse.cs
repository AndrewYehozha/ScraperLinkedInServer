using ScraperLinkedInServer.Models.Entities;

namespace ScraperLinkedInServer.Models.Response
{
    public class AuthorizationResponse : BaseResponse
    {
        public AccountViewModel Account { get; set; }
        public string Token { get; set; }
    }

    public class AuthorizationWindowsServiceResponse : BaseResponse
    {
        public string Token { get; set; }
    }
}