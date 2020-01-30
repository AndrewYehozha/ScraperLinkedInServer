using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.ObjectMappers;
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

            var result = await accountService.Authorization(request);

            return JsonSuccess(result);
        }

        public async Task<IHttpActionResult> Registration(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return JsonError(new RegistrationResponse { Message = ModelState?.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
            }

            var isExistAccount = await accountService.IsExistAccount(request.Email);
            if (isExistAccount)
            {
                return JsonError(new RegistrationResponse { Message = "Account is already registered" });
            }

            var accountVM = Mapper.Instance.Map<RegistrationRequest, AccountViewModel>(request);
            accountVM = await accountService.InsertAccountAsync(accountVM);
            var token = TokenManager.GenerateToken(accountVM);

            return JsonSuccess(new RegistrationResponse
            {
                Account = accountVM,
                Token = token
            });
        }
    }
}
