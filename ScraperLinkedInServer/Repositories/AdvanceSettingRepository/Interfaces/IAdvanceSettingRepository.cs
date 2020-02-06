using ScraperLinkedInServer.Database;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.AdvanceSettingRepository.Interfaces
{
    public interface IAdvanceSettingRepository
    {
        Task<AdvanceSetting> GetAdvanceSettingByAccountId(int accountId);

        Task InsertAdvanceSettingAsync(AdvanceSetting advanceSetting);

        Task UpdateAdvanceSettingAsync(AdvanceSetting advanceSetting);
    }
}
