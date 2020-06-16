using ScraperLinkedInServer.Models.Types;

namespace ScraperLinkedInServer.Models.Entities
{
    public class SearchSuitableProfilesViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Job { get; set; }
        public string PersonLinkedIn { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string EmailStatus { get; set; }
        public string TechStack { get; set; }
        public int CompanyID { get; set; }
        public ProfileStatus ProfileStatus { get; set; }
        public string DateTimeCreation { get; set; }
    }
}