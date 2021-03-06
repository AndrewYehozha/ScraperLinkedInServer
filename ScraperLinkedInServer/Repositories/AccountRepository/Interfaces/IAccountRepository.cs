﻿using ScraperLinkedInServer.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.AccountRepository.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByEmailAsync(string email);

        Task<IEnumerable<int>> GetActiveAccountsIdsAsync();

        Task<Account> InsertAccountAsync(Account account);

        Task UpdateAccountAsync(Account account);

        Task DeleteAccountAsync(int accountId);

        Task ChangeAccountBlockAsync(int accountId, bool isBlocked);

        Task ChangeAccountRoleAsync(int accountId, string role);

        Task<Account> GetAccountByIdAsync(int id);
    }
}
