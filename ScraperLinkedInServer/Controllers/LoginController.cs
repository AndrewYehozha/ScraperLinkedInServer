using ScraperLinkedInServer.Models;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Services.AccountService.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ScraperLinkedInApiController
    {
        private IAccountService accountService;

        public LoginController(
            IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(AuthorizationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return JsonError(new AuthorizationResponse { Message = ModelState?.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
            }

            var account = await accountService.GetAccountByEmailAsync(request.Email);

            var message = account.IsValid();
            if (!string.IsNullOrEmpty(message))
            {
                return JsonError(new AuthorizationResponse { Message = message });
            }

            var isCorrectPassword = accountService.CheckUserCorrectPassword(request.Password, account.Password);
            if (!isCorrectPassword)
            {
                return JsonError(new AuthorizationResponse { Message = "Incorrect password" });
            }

            account.Password = string.Empty;
            var token = TokenManager.GenerateToken(account);

            return JsonSuccess(new AuthorizationResponse
            {
                Account = account,
                Token = token
            });
        }

        public async Task<IHttpActionResult> Registration(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return JsonError(new RegistrationResponse { Message = ModelState?.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
            }

            var account = await accountService.GetAccountByEmailAsync(request.Email);
            if (account != null)
            {
                return JsonError(new RegistrationResponse { Message = "Account is already registered" });
            }

            account = await accountService.InsertAccountAsync(request);
            var token = TokenManager.GenerateToken(account);

            return JsonSuccess(new RegistrationResponse
            {
                Account = account,
                Token = token
            });
        }
    }
}
