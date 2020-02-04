using ScraperLinkedInServer.Database;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.SettingRepository.Interfaces
{
    public interface ISettingRepository
    {
        Task<Setting> GetSettingAsync(int accountId);

        Task InsertSettingAsync(Setting setting);

        Task UpdateSettingAsync(Setting setting);
    }
}
