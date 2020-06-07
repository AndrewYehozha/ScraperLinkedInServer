namespace ScraperLinkedInServer.Models.Request
{
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmedPassword { get; set; }
    }
}