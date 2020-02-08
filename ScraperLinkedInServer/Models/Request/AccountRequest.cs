using ScraperLinkedInServer.Models.Entities;

namespace ScraperLinkedInServer.Models.Request
{
    public class AccountRequest
    {
        public AccountViewModel AccountViewModel { get; set; }
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