using ScraperLinkedInServer.Database;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.AccountRepository.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByEmailAsync(string email);
        Task<Account> InsertAccountAsync(Account account);
    }
}
