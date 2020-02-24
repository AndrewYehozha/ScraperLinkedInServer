using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.AccountService.Interfaces
{
    public interface IAccountService
    {
        Task<AccountResponse> GetAccountByIdAsync(int id);

        Task<AuthorizationResponse> Authorization(AuthorizationRequest request);

        Task<AuthorizationWindowsServiceResponse> WindowsServiceAuthorization(AuthorizationWindowsServiceRequest request);

        Task<bool> IsExistAccount(string email);

        Task<AccountViewModel> InsertAccountAsync(AccountViewModel accountVM);

        bool CheckAccountCorrectPassword(string enteredPassword, string hashUserPassword);

        Task<AccountResponse> UpdateAccountAsync(AccountViewModel accountVM);

        Task DeleteAccountAsync(int accountId);

        Task ChangeAccountBlockAsync(ChangeAccountBlockRequest request);

        Task ChangeAccountRoleAsync(ChangeAccountRoleRequest request);
    }
}
