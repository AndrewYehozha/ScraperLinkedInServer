using ScraperLinkedInServer.Models.Entities;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.SettingService.Interfaces
{
    public interface ISettingService
    {
        Task<SettingViewModel> GetSettingByAccountIdAsync(int accountId);

        Task InsertDefaultSettingAsync(int accountId);

        Task UpdateSettingAsync(SettingViewModel settingVM);
    }
}
