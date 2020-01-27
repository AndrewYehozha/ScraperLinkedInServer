using System.ComponentModel.DataAnnotations;

namespace ScraperLinkedInServer.Models.Request
{
    public class RegistrationRequest
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid Phone")]
        public string Phone { get; set; }

        public System.DateTime DateOfBirthday { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 7, ErrorMessage = "Password should be longer than 6 characters")]
        public string Password { get; set; }
    }
}