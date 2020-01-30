using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.AccountService.Interfaces
{
    public interface IAccountService
    {
        Task<AuthorizationResponse> Authorization(AuthorizationRequest request);

        Task<bool> IsExistAccount(string email);

        Task<AccountViewModel> InsertAccountAsync(AccountViewModel request);

        bool CheckUserCorrectPassword(string enteredPassword, string hashUserPassword);
    }
}
