using ScraperLinkedInServer.Database;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.AdvanceSettingRepository.Interfaces
{
    public interface IAdvanceSettingRepository
    {
        Task<AdvanceSetting> GetAdvanceSetting(int accountId);

        Task InsertAdvanceSettingAsync(AdvanceSetting advanceSetting);

        Task UpdateAdvanceSettingAsync(AdvanceSetting advanceSetting);
    }
}
