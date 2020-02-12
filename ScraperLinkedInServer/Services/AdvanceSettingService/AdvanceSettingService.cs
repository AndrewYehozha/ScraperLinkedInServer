using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.ObjectMappers;
using ScraperLinkedInServer.Repositories.AdvanceSettingRepository.Interfaces;
using ScraperLinkedInServer.Services.AdvanceSettingService.Interfaces;
using System;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.AdvanceSettingService
{
    public class AdvanceSettingService : IAdvanceSettingService
    {
        private readonly IAdvanceSettingRepository _advanceSettingRepository;

        public AdvanceSettingService(
            IAdvanceSettingRepository advanceSettingRepository
        )
        {
            _advanceSettingRepository = advanceSettingRepository;
        }

        public async Task<AdvanceSettingViewModel> GetAdvanceSettingByAccountId(int accountId)
        {
            var advenceSettingDb = await _advanceSettingRepository.GetAdvanceSettingByAccountId(accountId);
            return Mapper.Instance.Map<AdvanceSetting, AdvanceSettingViewModel>(advenceSettingDb);
        }

        public async Task InsertDefaultAdvanceSettingAsync(int accountId)
        {
            var defaultAdvanceSetting = new AdvanceSetting
            {
                TimeStart = new TimeSpan(0, 1, 0),
                IntervalType = (int)IntervalTypesSettings.Days,
                IntervalValue = 1,
                CompanyBatchSize = 2,
                ProfileBatchSize = 50,
                AccountId = accountId
            };

            await _advanceSettingRepository.InsertAdvanceSettingAsync(defaultAdvanceSetting);
        }

        public async Task UpdateAdvanceSettingAsync(AdvanceSettingViewModel advanceSettingVM)
        {
            var advanceSettingDB = Mapper.Instance.Map<AdvanceSettingViewModel, AdvanceSetting>(advanceSettingVM);
            await _advanceSettingRepository.UpdateAdvanceSettingAsync(advanceSettingDB);
        }
    }
}