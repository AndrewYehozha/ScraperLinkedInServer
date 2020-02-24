using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.ObjectMappers;
using ScraperLinkedInServer.Repositories.SettingRepository.Interfaces;
using ScraperLinkedInServer.Services.SettingService.Interfaces;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.SettingService
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;

        public SettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public async Task<SettingsViewModel> GetSettingByAccountIdAsync(int accountId)
        {
            var settingDb = await _settingRepository.GetSettingByAccountIdAsync(accountId);
            return Mapper.Instance.Map<Setting, SettingsViewModel>(settingDb);
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
                ScraperStatusID = (int)Models.Types.ScraperStatus.OFF,
                AccountId = accountId
            };

            await _settingRepository.InsertSettingAsync(defaultSetting);
        }

        public async Task UpdateSettingAsync(SettingsViewModel settingVM)
        {
            var settingDb = Mapper.Instance.Map<SettingsViewModel, Setting>(settingVM);
            await _settingRepository.UpdateSettingAsync(settingDb);
        }

        public async Task UpdateScraperStatus(int accountId, Models.Types.ScraperStatus status)
        {
            await _settingRepository.UpdateScraperStatus(accountId, status);
        }
    }
}