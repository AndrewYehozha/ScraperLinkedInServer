using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Repositories.AccountRepository.Interfaces;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.AccountRepository
{
    public class AccountRepository : IAccountRepository
    {
        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.Accounts.Where(x => x.Email == email).FirstOrDefaultAsync();
            }
        }

        public async Task<Account> InsertAccountAsync(Account account)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var accountDb = db.Accounts.Add(account);
                await db.SaveChangesAsync();

                return accountDb;
            }
        }

    }
}