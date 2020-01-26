using ScraperLinkedInServer.Database;

namespace ScraperLinkedInServer.Services.AccountService.Interfaces
{
    public interface IAccountService
    {
        Profile GetAccountByEmail(string email);
    }
}
