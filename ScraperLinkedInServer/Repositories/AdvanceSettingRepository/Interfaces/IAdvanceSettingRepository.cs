using ScraperLinkedInServer.Database;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.AdvanceSettingRepository.Interfaces
{
    public interface IAdvanceSettingRepository
    {
        Task InsertAdvanceSettingAsync(AdvanceSetting advanceSetting);
    }
}
