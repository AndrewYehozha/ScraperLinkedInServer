using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Utilities;
using System;
using System.Linq;
using System.Web.Http;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ScraperLinkedInApiController
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(AuthorizationRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    response = ModelState?.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage
                });
            }

            //var account = accountReadService.SearchAccountByEmail(model.Email);

            //if (account == null)
            //{
            //return Json(new
            //{
            //    success = false,
            //    response = "Account not registered"
            //});
            //}
            //else
            //{
            //try
            //{
            //    var isCorrectPassword = userReadService.CheckUserCorrectPassword(model.Password, user.Password);

            //    if (!isCorrectPassword)
            //    {
            //return Json(new
            //{
            //    success = false,
            //    response = "Incorrect password"
            //});
            //    }


            var account = new Account
            {
                AccountId = 222,
                FirstName = "Andrew",
                LastName = "Test",
                Email = "test@gmail.com",
                RoleId = 1,
                Role = Role.User
            };

            var token = TokenManager.GenerateToken(account);

            return Json(new
            {
                success = true,
                response = token
            });
            //}
            //catch (Exception ex)
            //{
            //return Json(new
            //{
            //    success = false,
            //    response = ex.Message
            //});
            //}
        }
    }
}
