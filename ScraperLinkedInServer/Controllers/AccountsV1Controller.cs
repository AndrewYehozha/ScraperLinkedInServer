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
        private readonly IAccountService _accountService;

        public AccountsV1Controller(
            IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("signin")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SignIn(AuthorizationRequest request)
        {
            var response = await _accountService.Authorization(request);

            return Ok(response);
        }

        [HttpPost]
        [Route("signin/windows-service")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> WindowsServiceSignIn(AuthorizationWindowsServiceRequest request)
        {
            var result = await _accountService.WindowsServiceAuthorization(request);

            return Ok(result);
        }

        [HttpPost]
        [Route("signup")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SignUp(RegistrationRequest request)
        {
            var response = new RegistrationResponse();

            var isExistAccount = await _accountService.IsExistAccount(request.Email, request.Phone);
            if (isExistAccount)
            {
                response.ErrorMessage = "Account with that email or phone is already registered";
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else {
                var accountVM = Mapper.Instance.Map<RegistrationRequest, AccountViewModel>(request);
                accountVM = await _accountService.InsertAccountAsync(accountVM);
                var tokenResponse = TokenManager.GenerateToken(accountVM, Roles.User);

                response.Account = accountVM;
                response.Token = tokenResponse.Token;
                response.TokenExpires = tokenResponse.Expires;
                response.StatusCode = (int)HttpStatusCode.OK;
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("active/ids")]
        [Authorize]
        public async Task<IHttpActionResult> GetActiveAccountsIdsAsync()
        {
            var response = new AccountsIdsResponse();

            var isAdmin = bool.Parse(Identity.ToIsAdmin());
            if (!isAdmin)
            {
                response.ErrorMessage = "Not permissions";
                response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
            else
            {
                response.Ids = await _accountService.GetActiveAccountsIdsAsync();
                response.StatusCode = (int)HttpStatusCode.OK;
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("account")]
        [Authorize]
        public async Task<IHttpActionResult> GetAccountByIdAsync()
        {
            var response = new AccountResponse();

            var accountId = Identity.ToAccountID();
            response = await _accountService.GetAccountByIdAsync(accountId);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPut]
        [Route("account")]
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
                response = await _accountService.UpdateAccountAsync(request.AccountViewModel);
                response.StatusCode = (int)HttpStatusCode.OK;
            }

            return Ok(response);
        }

        [HttpDelete]
        [Route("account")]
        [Authorize]
        public async Task<IHttpActionResult> DeleteAccountAsync()
        {
            var response = new AccountResponse();

            var accountId = Identity.ToAccountID();
            await _accountService.DeleteAccountAsync(accountId);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPut]
        [Route("account/change-password")]
        public async Task<IHttpActionResult> ChangeAccountRoleAsync(ChangePasswordRequest request)
        {
            var accountId = Identity.ToAccountID();
            var response = await _accountService.ChangePasswordByAccountIdAsync(accountId, request);

            return Ok(response);
        }


        //Admin functionality

        [HttpPut]
        [Route("account/role")]
        [Authorize(Roles = Roles.Admin)]
        public  async Task<IHttpActionResult> ChangeAccountRoleAsync(ChangeAccountRoleRequest request)
        {
            var response = new AccountResponse();

            await _accountService.ChangeAccountRoleAsync(request);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }

        [HttpPut]
        [Route("account/block")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IHttpActionResult> ChangeAccountBlockAsync(ChangeAccountBlockRequest request)
        {
            var response = new AccountResponse();

            await _accountService.ChangeAccountBlockAsync(request);
            response.StatusCode = (int)HttpStatusCode.OK;

            return Ok(response);
        }
    }
}
