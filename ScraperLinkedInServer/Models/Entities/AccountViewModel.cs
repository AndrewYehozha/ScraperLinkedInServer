namespace ScraperLinkedInServer.Models.Entities
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public System.DateTime DateOfBirthday { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBlocked { get; set; }
    }
}