using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Repositories.AdvanceSettingRepository.Interfaces;
using ScraperLinkedInServer.Services.AdvanceSettingService.Interfaces;
using System;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.AdvanceSettingService
{
    public class AdvanceSettingService : IAdvanceSettingService
    {
        private readonly IAdvanceSettingRepository advanceSettingRepository;

        public AdvanceSettingService(
            IAdvanceSettingRepository advanceSettingRepository
        )
        {
            this.advanceSettingRepository = advanceSettingRepository;
        }

        public async Task InsertDefaultAdvanceSettingAsync(int accountId)
        {
            var defaultAdvanceSetting = new AdvanceSetting {
                TimeStart = new TimeSpan(0, 1, 0),
                IntervalType = (int)IntervalTypesSettings.Days,
                IntervalValue = 1,
                CompanyBatchSize = 5,
                ProfileBatchSize = 500,
                AccountId = accountId
            };

            await advanceSettingRepository.InsertAdvanceSettingAsync(defaultAdvanceSetting);
        }
    }
}