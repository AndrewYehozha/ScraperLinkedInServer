using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraperLinkedInServer.Models.Entities
{
    public class Account
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int RoleId { get; set; }

        public string Role { get; set; }

        //public DateTime Birthday { get; set; }

        //public string Password { get; set; }

        //public string Phone { get; set; }
    }
}