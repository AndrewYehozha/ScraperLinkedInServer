using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.ProfileService.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileViewModel> GetProfileByIdAsync(int id, int accountId);

        Task<IEnumerable<ProfileViewModel>> GetProfilesForSearchAsync(int accountId, int profileBatchSize);

        Task<int> GetCountProfilesInProcessAsync(int accountId);

        Task<int> GetCountNewProfilesAsync(int accountId);

        Task InsertProfilesAsync(IEnumerable<ProfileViewModel> profilesVM);

        Task UpdateProfilesAsync(IEnumerable<ProfileViewModel> profilesVM);

        Task UpdateProfilesExecutionStatusByCompanyIdAsync(int accountId, int companyId, ExecutionStatuses executionStatus);
    }
}
