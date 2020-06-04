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
using ScraperLinkedInServer.Services.PaymentService.Interfaces;
using System.Net;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly ISettingService _settingService;
        private readonly IAdvanceSettingService _advanceSettingService;
        private readonly IPaymentService _paymentService;

        private readonly IAccountRepository _accountRepository;

        public AccountService(
            ISettingService settingService,
            IAdvanceSettingService advanceSettingService,
            IAccountRepository accountRepository,
            IPaymentService paymentService)
        {
            _settingService = settingService;
            _advanceSettingService = advanceSettingService;
            _paymentService = paymentService;

            _accountRepository = accountRepository;
        }

        public async Task<AccountResponse> GetAccountByIdAsync(int id)
        {
            var response = new AccountResponse();
            var accountDb = await _accountRepository.GetAccountByIdAsync(id);

            response.ErrorMessage = accountDb.IsValid();
            response.AccountViewModel = Mapper.Instance.Map<Account, AccountViewModel>(accountDb);
            response.AccountViewModel.Password = string.Empty;

            return response;
        }

        public async Task<IEnumerable<int>> GetActiveAccountsIdsAsync()
        {
            return await _accountRepository.GetActiveAccountsIdsAsync();
        }

        public async Task<AuthorizationResponse> Authorization(AuthorizationRequest request)
        {
            var response = new AuthorizationResponse();
            var account = await _accountRepository.GetAccountByEmailOrPhoneAsync(request.Email);
            
            var message = account.IsValid();
            if (!string.IsNullOrEmpty(message))
            {
                response.ErrorMessage = message;
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return response;
            }

            var isCorrectPassword = CheckAccountCorrectPassword(request.Password, account.Password);
            if (!isCorrectPassword)
            {
                response.ErrorMessage = "Incorrect password";
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return response;
            }

            var accountVM = Mapper.Instance.Map<Account, AccountViewModel>(account);
            var tokenResponse = TokenManager.GenerateToken(accountVM, Roles.User);

            response.Account = accountVM;
            response.Token = tokenResponse.Token;
            response.TokenExpires = tokenResponse.Expires;
            response.StatusCode = (int)HttpStatusCode.OK;

            return response;
        }

        public async Task<AuthorizationWindowsServiceResponse> WindowsServiceAuthorization(AuthorizationWindowsServiceRequest request)
        {
            var response = new AuthorizationWindowsServiceResponse();
            var payment = await _paymentService.GetPaymentByGuideAsync(request.Guid);

            if (payment == null)
            {
                response.ErrorMessage = $"Incorrect guid";
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return response;
            }

            var account = await _accountRepository.GetAccountByIdAsync(payment.AccountId);
            var accountVM = Mapper.Instance.Map<Account, AccountViewModel>(account);

            var tokenResponse = TokenManager.GenerateToken(accountVM, Roles.WindowsService);
            response.Token = tokenResponse.Token;
            response.TokenExpires = tokenResponse.Expires;

            response.StatusCode = (int)HttpStatusCode.OK;

            return response;
        }

        public async Task<bool> IsExistAccount(string email, string phone)
        {
            return await _accountRepository.GetAccountByEmailOrPhoneAsync(email, phone) != null;
        }

        public async Task<AccountViewModel> InsertAccountAsync(AccountViewModel accountVM)
        {
            var accountDb = Mapper.Instance.Map<AccountViewModel, Account>(accountVM);
            accountDb.Password = HashPassword(accountDb.Password);
            accountDb.Role = Roles.User;

            accountDb = await _accountRepository.InsertAccountAsync(accountDb);
            await _settingService.InsertDefaultSettingAsync(accountDb.Id);
            await _advanceSettingService.InsertDefaultAdvanceSettingAsync(accountDb.Id);

            var accountResponse = Mapper.Instance.Map<Account, AccountViewModel>(accountDb);
            accountResponse.Password = string.Empty;

            return accountResponse;
        }

        public async Task<AccountResponse> UpdateAccountAsync(AccountViewModel accountVM)
        {
            var response = new AccountResponse();
            var account = Mapper.Instance.Map<AccountViewModel, Account>(accountVM);

            var accountDb = await _accountRepository.GetAccountByIdAsync(accountVM.Id);
            var message = accountDb.IsValid();
            if (!string.IsNullOrEmpty(message))
            {
                response.ErrorMessage = message;
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                await _accountRepository.UpdateAccountAsync(account);
                response.StatusCode = (int)HttpStatusCode.OK;
            }

            return response;
        }

        public async Task ChangeAccountRoleAsync(ChangeAccountRoleRequest request)
        {
            await _accountRepository.ChangeAccountRoleAsync(request.AccountId, request.Role);
        }

        public async Task ChangeAccountBlockAsync(ChangeAccountBlockRequest request)
        {
            await _accountRepository.ChangeAccountBlockAsync(request.AccountId, request.IsBlocked);
        }

        public async Task DeleteAccountAsync(int accountId)
        {
            await _accountRepository.DeleteAccountAsync(accountId);
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