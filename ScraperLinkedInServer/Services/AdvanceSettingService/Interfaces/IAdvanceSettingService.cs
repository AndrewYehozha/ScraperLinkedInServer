using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.AdvanceSettingService.Interfaces
{
    public interface IAdvanceSettingService
    {
        Task InsertDefaultAdvanceSettingAsync(int accountId);
    }
}
