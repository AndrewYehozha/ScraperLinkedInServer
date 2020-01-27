using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Request;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.AccountService.Interfaces
{
    public interface IAccountService
    {
        Task<Account> GetAccountByEmailAsync(string email);

        Task<Account> InsertAccountAsync(RegistrationRequest request);

        bool CheckUserCorrectPassword(string enteredPassword, string hashUserPassword);
    }
}
