using ScraperLinkedInServer.Database;
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

        public async Task<AccountBaseResponse> GetAccountByIdAsync(int accountId)
        {
            var response = new AccountBaseResponse();
            var accountDb = await accountRepository.GetAccountByIdAsync(accountId);

            response.Message = accountDb.IsValid();
            response.AccountViewModel = Mapper.Instance.Map<Account, AccountViewModel>(accountDb);
            response.AccountViewModel.Password = string.Empty;

            return response;
        }

        public async Task<AuthorizationResponse> Authorization(AuthorizationRequest request)
        {
            var account = await accountRepository.GetAccountByEmailAsync(request.Email);

            var message = account.IsValid();
            if (!string.IsNullOrEmpty(message))
            {
                return new AuthorizationResponse { Message = message };
            }

            var isCorrectPassword = CheckAccountCorrectPassword(request.Password, account.Password);
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

        public async Task<AccountViewModel> InsertAccountAsync(AccountViewModel accountVM)
        {
            var accountDb = Mapper.Instance.Map<AccountViewModel, Account>(accountVM);
            accountDb.Password = HashPassword(accountDb.Password);
            accountDb.Role = Roles.User;

            accountDb = await accountRepository.InsertAccountAsync(accountDb);
            await settingService.InsertDefaultSettingAsync(accountDb.Id);
            await advanceSettingService.InsertDefaultAdvanceSettingAsync(accountDb.Id);

            var accountResponse = Mapper.Instance.Map<Account, AccountViewModel>(accountDb);
            accountResponse.Password = string.Empty;

            return accountResponse;
        }

        public async Task<AccountBaseResponse> UpdateAccountAsync(AccountViewModel accountVM)
        {
            var response = new AccountBaseResponse();
            var account = Mapper.Instance.Map<AccountViewModel, Account>(accountVM);

            var accountDb = await accountRepository.GetAccountByIdAsync(accountVM.Id);
            var message = accountDb.IsValid();
            if (!string.IsNullOrEmpty(message))
            {
                response.Message = message;
            }
            else
            {
                await accountRepository.UpdateAccountAsync(account);
            }

            return response;
        }

        public async Task ChangeAccountRoleAsync(ChangeAccountRoleRequest request)
        {
            await accountRepository.ChangeAccountRoleAsync(request.AccountId, request.Role);
        }

        public async Task ChangeAccountBlockAsync(ChangeAccountBlockRequest request)
        {
            await accountRepository.ChangeAccountBlockAsync(request.AccountId, request.IsBlocked);
        }

        public async Task DeleteAccountAsync(int accountId)
        {
            await accountRepository.DeleteAccountAsync(accountId);
        }

        public bool CheckAccountCorrectPassword(string enteredPassword, string hashAccountPassword)
        {
            return HashPassword(enteredPassword) == hashAccountPassword ? true : false;
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