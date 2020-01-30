﻿using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Services.AccountService.Interfaces;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;
using ScraperLinkedInServer.ObjectMappers;
using ScraperLinkedInServer.Repositories.AccountRepository.Interfaces;
using ScraperLinkedInServer.Services.SettingService.Interfaces;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Services.AdvanceSettingService.Interfaces;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Models;

namespace ScraperLinkedInServer.Services.AccountService
{
    public class AccountService : IAccountService
    {
        ISettingService settingService;
        IAdvanceSettingService advanceSettingService;

        IAccountRepository accountRepository;

        public AccountService(
            ISettingService settingService,
            IAdvanceSettingService advanceSettingService,
            IAccountRepository accountRepository
        )
        {
            this.settingService = settingService;
            this.advanceSettingService = advanceSettingService;

            this.accountRepository = accountRepository;
        }

        public async Task<AuthorizationResponse> Authorization(AuthorizationRequest request)
        {
            var account = await accountRepository.GetAccountByEmailAsync(request.Email);

            var message = account.IsValid();
            if (!string.IsNullOrEmpty(message))
            {
                return new AuthorizationResponse { Message = message };
            }

            var isCorrectPassword = CheckUserCorrectPassword(request.Password, account.Password);
            if (!isCorrectPassword)
            {
                return new AuthorizationResponse { Message = "Incorrect password" };
            }

            var accountVM = Mapper.Instance.Map<Account, AccountViewModel>(account);
            var token = TokenManager.GenerateToken(accountVM);

            return new AuthorizationResponse { Account = accountVM, Token = token };
        }

        public async Task<bool> IsExistAccount(string email)
        {
            return await accountRepository.GetAccountByEmailAsync(email) != null;
        }

        public async Task<AccountViewModel> InsertAccountAsync(AccountViewModel request)
        {
            var accountDb = Mapper.Instance.Map<AccountViewModel, Account>(request);
            accountDb.Password = HashPassword(accountDb.Password);
            accountDb.Role = Role.User;

            accountDb = await accountRepository.InsertAccountAsync(accountDb);
            await settingService.InsertDefaultSettingAsync(accountDb.Id);
            await advanceSettingService.InsertDefaultAdvanceSettingAsync(accountDb.Id);

            var response = Mapper.Instance.Map<Account, AccountViewModel>(accountDb);
            response.Password = string.Empty;

            return response;
        }


        public bool CheckUserCorrectPassword(string enteredPassword, string hashUserPassword)
        {
            return HashPassword(enteredPassword) == hashUserPassword ? true : false;
        }

        //Utils
        private string HashPassword(string password)
        {
            var CSP = new MD5CryptoServiceProvider();
            var byteHash = CSP.ComputeHash(Encoding.Unicode.GetBytes(password));
            var hash = new StringBuilder();

            foreach (byte b in byteHash)
            {
                hash.Append(string.Format("{0:x2}", b));
            }

            return hash.ToString();
        }
    }
}