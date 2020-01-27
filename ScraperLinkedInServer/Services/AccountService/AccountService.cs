using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Services.AccountService.Interfaces;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.ObjectMappers;
using ScraperLinkedInServer.Repositories.AccountRepository.Interfaces;
using ScraperLinkedInServer.Services.SettingService.Interfaces;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Services.AdvanceSettingService.Interfaces;

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

        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            return await accountRepository.GetAccountByEmailAsync(email);
        }

        public async Task<Account> InsertAccountAsync(RegistrationRequest request)
        {
            var accountDb = Mapper.Instance.Map<RegistrationRequest, Account>(request);
            accountDb.Password = HashPassword(accountDb.Password);
            accountDb.Role = Role.User;
            var response = await accountRepository.InsertAccountAsync(accountDb);
            await settingService.InsertDefaultSettingAsync(response.Id);
            await advanceSettingService.InsertDefaultAdvanceSettingAsync(response.Id);

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