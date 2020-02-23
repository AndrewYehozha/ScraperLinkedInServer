using ScraperLinkedInServer.Models.Entities;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.SettingService.Interfaces
{
    public interface ISettingService
    {
        Task<SettingsViewModel> GetSettingByAccountIdAsync(int accountId);

        Task InsertDefaultSettingAsync(int accountId);

        Task UpdateSettingAsync(SettingsViewModel settingVM);

        Task UpdateScraperStatus(int accountId, Models.Types.ScraperStatus status);
    }
}
