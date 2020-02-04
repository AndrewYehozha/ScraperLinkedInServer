using ScraperLinkedInServer.Database;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.AccountRepository.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByEmailAsync(string email);

        Task<Account> InsertAccountAsync(Account account);

        Task UpdateAccountAsync(Account account);

        Task ChangeRoleAccountAsync(int accountId, string role);

        Task ChangeBlockAccountAsync(int accountId, bool isBlocked);

        Task DeleteAccountAsync(int accountId);
    }
}
