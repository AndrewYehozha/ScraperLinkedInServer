using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Response;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.AdvanceSettingService.Interfaces
{
    public interface IAdvanceSettingService
    {
        Task<AdvanceSettingViewModel> GetAdvanceSettingByAccountId(int accountId);

        Task InsertDefaultAdvanceSettingAsync(int accountId);

        Task UpdateAdvanceSettingAsync(AdvanceSettingViewModel advanceSettingVM);
    }
}
