using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Repositories.AdvanceSettingRepository.Interfaces;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.AdvanceSettingRepository
{
    public class AdvanceSettingRepository : IAdvanceSettingRepository
    {
        public async Task InsertAdvanceSettingAsync(AdvanceSetting advanceSetting)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                db.AdvanceSettings.Add(advanceSetting);
                await db.SaveChangesAsync();
            }
        }
    }
}