using ScraperLinkedInServer.Models.Entities;

namespace ScraperLinkedInServer.Models.Response
{
    public class SettingsResponse : BaseResponse
    {
        public SettingsViewModel SettingsViewModel { get; set; }
    }
}