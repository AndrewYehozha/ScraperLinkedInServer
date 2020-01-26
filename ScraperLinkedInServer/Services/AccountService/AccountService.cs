//using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Services.AccountService.Interfaces;
using System.Linq;
using System;

namespace ScraperLinkedInServer.Services.AccountService
{
    public class AccountService : IAccountService
    {
        //private ScraperLinkedInContext dbContext;
        public AccountService()
        {
            //this.dbContext = new ScraperLinkedInContext();
        }
        public Profile GetAccountByEmail(string email)
        {
            //var x = dbContext.ProfileStatuses.FirstOrDefault();
            using (var db = new ScraperLinkedInDBEntities())
            {
                var result = db.ProfileStatuses.FirstOrDefault();

            }
            return new Profile
            {
                Id = 222,
                FirstName = "Andrew",
                LastName = "Test",
                //Email = "test@gmail.com",
               // RoleId = 1,
               // Role = Role.User
            };
        }
    }
}