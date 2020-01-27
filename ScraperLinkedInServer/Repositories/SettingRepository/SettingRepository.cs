using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Repositories.SettingRepository.Interfaces;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.SettingRepository
{
    public class SettingRepository : ISettingRepository
    {
        public async Task InsertSettingAsync(Setting setting)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                db.Settings.Add(setting);
                await db.SaveChangesAsync();
            }
        }
    }
}