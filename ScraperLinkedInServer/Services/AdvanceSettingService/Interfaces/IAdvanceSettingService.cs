using ScraperLinkedInServer.Models.Entities;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.AdvanceSettingService.Interfaces
{
    public interface IAdvanceSettingService
    {
        Task InsertDefaultAdvanceSettingAsync(int accountId);

        Task UpdateAdvanceSettingAsync(AdvanceSettingViewModel advanceSetting);
    }
}
