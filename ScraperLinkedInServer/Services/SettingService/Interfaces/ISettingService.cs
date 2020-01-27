using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.SettingService.Interfaces
{
    public interface ISettingService
    {
        Task InsertDefaultSettingAsync(int accountId);
    }
}
