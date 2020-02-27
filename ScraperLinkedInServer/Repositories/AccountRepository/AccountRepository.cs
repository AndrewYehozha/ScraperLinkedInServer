using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Repositories.AccountRepository.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.AccountRepository
{
    public class AccountRepository : IAccountRepository
    {
        public async Task<Account> GetAccountByIdAsync(int id)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.Accounts.Where(x => x.Id == id).FirstOrDefaultAsync();
            }
        }

        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.Accounts.Where(x => x.Email == email).FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<int>> GetActiveAccountsIdsAsync()
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.Accounts.Where(x => !x.IsBlocked && !x.IsDeleted).Select(s => s.Id).ToListAsync();
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

        public async Task UpdateAccountAsync(Account account)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var accountDb = db.Accounts.Where(x => x.Id == account.Id).FirstOrDefault();

                accountDb.FirstName = account.FirstName;
                accountDb.LastName = account.LastName;
                accountDb.Email = account.Email;
                accountDb.Phone = account.Phone;
                accountDb.DateOfBirthday = account.DateOfBirthday;

                await db.SaveChangesAsync();
            }
        }

        public async Task ChangeAccountRoleAsync(int accountId, string role)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var accountDb = db.Accounts.Where(x => x.Id == accountId).FirstOrDefault();
                accountDb.Role = role;
                await db.SaveChangesAsync();
            }
        }

        public async Task ChangeAccountBlockAsync(int accountId, bool isBlocked)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var accountDb = db.Accounts.Where(x => x.Id == accountId).FirstOrDefault();
                accountDb.IsBlocked = isBlocked;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAccountAsync(int accountId)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var accountDb = db.Accounts.Where(x => x.Id == accountId).FirstOrDefault();
                accountDb.IsDeleted = true;
                await db.SaveChangesAsync();
            }
        }
    }
}