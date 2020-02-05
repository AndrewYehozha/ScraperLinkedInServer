using ScraperLinkedInServer.Models.Response;

namespace ScraperLinkedInServer.Models.Request
{
    public class AccountRequest : AccountBaseResponse
    {
    }

    public class ChangeAccountRoleRequest
    {
        public int AccountId { get; set; }
        public string Role { get; set; }
    }

    public class ChangeAccountBlockRequest
    {
        public int AccountId { get; set; }
        public bool IsBlocked { get; set; }
    }
}