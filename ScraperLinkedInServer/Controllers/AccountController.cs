using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.ObjectMappers;
using ScraperLinkedInServer.Services.AccountService.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/v1/accounts")]
    public class AccountController : ScraperLinkedInApiController
    {
        private IAccountService accountService;

        public AccountController(
            IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        [Route("/signin")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SignIn(AuthorizationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return JsonError(new AuthorizationResponse { Message = ModelState?.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
            }

            var result = await accountService.Authorization(request);

            return JsonSuccess(result);
        }

        [HttpPost]
        [Route("/signup")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SignUp(RegistrationRequest request)
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

        [HttpGet]
        [Route("/{id}")]
        [Authorize]
        public async Task<IHttpActionResult> GetAccountByIdAsync(int id)
        {
            var response = await accountService.GetAccountByIdAsync(id);

            return JsonSuccess(response);
        }

        [HttpPut]
        [Route("/")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateAccountAsync(AccountRequest request)
        {
            var response = await accountService.UpdateAccountAsync(request.AccountViewModel);

            return JsonSuccess(response);
        }

        [HttpDelete]
        [Route("/{id}")]
        [Authorize]
        public async Task<IHttpActionResult> DeleteAccountAsync(int id)
        {
            var response = new AccountBaseResponse();

            await accountService.DeleteAccountAsync(id);

            return JsonSuccess(response);
        }

        [HttpPut]
        [Route("/account-management/role")]
        [Authorize(Roles = Roles.Admin)]
        public  async Task<IHttpActionResult> ChangeAccountRoleAsync(ChangeAccountRoleRequest request)
        {
            var response = new AccountBaseResponse();

            await accountService.ChangeAccountRoleAsync(request);

            return JsonSuccess(response);
        }

        [HttpPut]
        [Route("/account-management/block")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IHttpActionResult> ChangeAccountBlockAsync(ChangeAccountBlockRequest request)
        {
            var response = new AccountBaseResponse();

            await accountService.ChangeAccountBlockAsync(request);

            return JsonSuccess(response);
        }
    }
}
