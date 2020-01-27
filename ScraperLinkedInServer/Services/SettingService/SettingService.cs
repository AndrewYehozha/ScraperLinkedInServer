using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Repositories.SettingRepository.Interfaces;
using ScraperLinkedInServer.Services.SettingService.Interfaces;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.SettingService
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository settingRepository;

        public SettingService(
            ISettingRepository settingRepository
        )
        {
            this.settingRepository = settingRepository;
        }

        public async Task InsertDefaultSettingAsync(int accountId)
        {
            var defaultSetting = new Setting
            {
                Token = string.Empty,
                Password = string.Empty,
                Login = string.Empty,
                TechnologiesSearch = string.Empty,
                RolesSearch = string.Empty,
                ScraperStatusID = (int)ScraperStatuses.OFF,
                AccountId = accountId
            };

            await settingRepository.InsertSettingAsync(defaultSetting);
        }
    }
}