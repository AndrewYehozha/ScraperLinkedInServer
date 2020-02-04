using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Repositories.AdvanceSettingRepository.Interfaces;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.AdvanceSettingRepository
{
    public class AdvanceSettingRepository : IAdvanceSettingRepository
    {
        public async Task<AdvanceSetting> GetAdvanceSetting(int accountId)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.AdvanceSettings.Where(x => x.AccountId == accountId).FirstOrDefaultAsync();
            }
        }

        public async Task InsertAdvanceSettingAsync(AdvanceSetting advanceSetting)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                db.AdvanceSettings.Add(advanceSetting);
                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateAdvanceSettingAsync(AdvanceSetting advanceSetting)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var advanceSettingsDb = await db.AdvanceSettings.Where(x => x.Id == advanceSetting.Id).FirstOrDefaultAsync();

                advanceSettingsDb.TimeStart = advanceSetting.TimeStart;
                advanceSettingsDb.IntervalType = advanceSetting.IntervalType;
                advanceSettingsDb.IntervalValue = advanceSetting.IntervalValue;
                advanceSettingsDb.CompanyBatchSize = advanceSetting.CompanyBatchSize;
                advanceSettingsDb.ProfileBatchSize = advanceSetting.ProfileBatchSize;

                await db.SaveChangesAsync();
            }
        }
    }
}