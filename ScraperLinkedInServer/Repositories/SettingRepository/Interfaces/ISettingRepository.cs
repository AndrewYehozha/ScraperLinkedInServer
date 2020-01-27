using ScraperLinkedInServer.Database;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.SettingRepository.Interfaces
{
    public interface ISettingRepository
    {
        Task InsertSettingAsync(Setting setting);
    }
}
