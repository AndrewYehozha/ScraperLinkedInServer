using ScraperLinkedInServer.Extensions;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.ObjectMappers;
using ScraperLinkedInServer.Services.AccountService.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/v1/accounts")]
    public class AccountsV1Controller : ScraperLinkedInApiController
    {
        private readonly IAccountService accountService;

        public AccountsV1Controller(
            IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        [Route("signin")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SignIn(AuthorizationRequest request)
        {
            var response = await accountService.Authorization(request);

            return Ok(response);
        }

        [HttpPost]
        [Route("windows-service/signin")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> WindowsServiceSignIn(AuthorizationWindowsServiceRequest request)
        {
            var result = await accountService.WindowsServiceAuthorization(request);

            return Ok(result);
        }

        [HttpPost]
        [Route("signup")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SignUp(RegistrationRequest request)
        {
            var response = new RegistrationResponse();

            var isExistAccount = await accountService.IsExistAccount(request.Email);
            if (isExistAccount)
            {
                response.ErrorMessage = "Account is already registered";
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else {
                var accountVM = Mapper.Instance.Map<RegistrationRequest, AccountViewModel>(request);
                accountVM = await accountService.InsertAccountAsync(accountVM);
                var token = TokenManager.GenerateToken(accountVM);

                response.Account = accountVM;
                response.Token = token;
                response.StatusCode = (int)HttpStatusCode.OK;
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IHttpActionResult> GetAccountByIdAsync()
        {
            var response = new AccountResponse();

            var accountId = Identity.ToAccountID();
            response = await accountService.GetAccountByIdAsync(accountId);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPut]
        [Route("account-management")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateAccountAsync(AccountRequest request)
        {
            var response = new AccountResponse();

            var accountId = Identity.ToAccountID();
            if (request.AccountViewModel.Id != accountId)
            {
                response.ErrorMessage = "Not permissions";
                response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
            else
            {
                response = await accountService.UpdateAccountAsync(request.AccountViewModel);
                response.StatusCode = (int)HttpStatusCode.OK;
            }

            return Ok(response);
        }

        [HttpDelete]
        [Route("account-management")]
        [Authorize]
        public async Task<IHttpActionResult> DeleteAccountAsync()
        {
            var response = new AccountResponse();

            var accountId = Identity.ToAccountID();
            await accountService.DeleteAccountAsync(accountId);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }


        //Admin functionality

        [HttpPut]
        [Route("account-management/role")]
        [Authorize(Roles = Roles.Admin)]
        public  async Task<IHttpActionResult> ChangeAccountRoleAsync(ChangeAccountRoleRequest request)
        {
            var response = new AccountResponse();

            await accountService.ChangeAccountRoleAsync(request);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPut]
        [Route("account-management/block")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IHttpActionResult> ChangeAccountBlockAsync(ChangeAccountBlockRequest request)
        {
            var response = new AccountResponse();

            await accountService.ChangeAccountBlockAsync(request);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }
    }
}
