using ScraperLinkedInServer.Models.Types;

namespace ScraperLinkedInServer.Models.Response
{
    public class SettingResponse
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public string TechnologiesSearch { get; set; }
        public string RolesSearch { get; set; }
        public ScraperStatuses ScraperStatus { get; set; }
        public int AccountId { get; set; }
    }
}