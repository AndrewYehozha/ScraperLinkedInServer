using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.ObjectMappers;
using ScraperLinkedInServer.Repositories.SettingRepository.Interfaces;
using ScraperLinkedInServer.Services.SettingService.Interfaces;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.SettingService
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;

        public SettingService(
            ISettingRepository settingRepository
        )
        {
            _settingRepository = settingRepository;
        }

        public async Task<SettingViewModel> GetSettingByAccountIdAsync(int accountId)
        {
            var settingDb = await _settingRepository.GetSettingByAccountIdAsync(accountId);
            return Mapper.Instance.Map<Setting, SettingViewModel>(settingDb);
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

            await _settingRepository.InsertSettingAsync(defaultSetting);
        }

        public async Task UpdateSettingAsync(SettingViewModel settingVM)
        {
            var settingDb = Mapper.Instance.Map<SettingViewModel, Setting>(settingVM);
            await _settingRepository.UpdateSettingAsync(settingDb);
        }
    }
}