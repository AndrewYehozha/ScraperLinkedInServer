using System.ComponentModel.DataAnnotations;

namespace ScraperLinkedInServer.Models.Request
{
    public class RegistrationRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public System.DateTime DateOfBirthday { get; set; }

        public string Password { get; set; }
    }
}