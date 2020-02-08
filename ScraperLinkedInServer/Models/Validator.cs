using ScraperLinkedInServer.Database;

namespace ScraperLinkedInServer.Models
{
    public static class Validator
    {
        public static string IsValid(this Account account)
        {
            if (account == null)
                return "Account not registered";

            if (account.IsBlocked)
                return "Account is blocked";

            if (account.IsDeleted)
                return "Account is deleted";

            return string.Empty;
        }
    }
}