using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Repositories.SettingRepository.Interfaces;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.SettingRepository
{
    public class SettingRepository : ISettingRepository
    {
        public async Task<Setting> GetSettingByAccountIdAsync(int accountId)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.Settings.Where(x => x.AccountId == accountId).FirstOrDefaultAsync();
            }
        }

        public async Task InsertSettingAsync(Setting setting)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                db.Settings.Add(setting);
                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateSettingAsync(Setting setting)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var settingDB = await db.Settings.Where(x => x.AccountId == setting.AccountId).FirstOrDefaultAsync();

                settingDB.Token = setting.Token;
                settingDB.Login = setting.Login;
                settingDB.Password = setting.Password;
                settingDB.TechnologiesSearch = setting.TechnologiesSearch;
                settingDB.RolesSearch = setting.RolesSearch;

                await db.SaveChangesAsync();
            }
        }
    }
}