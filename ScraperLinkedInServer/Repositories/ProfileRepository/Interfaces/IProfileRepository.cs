using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.ProfileRepository.Interfaces
{
    public interface IProfileRepository
    {
        Task<Profile> GetProfileByIdAsync(int id, int accountId);

        Task<IEnumerable<Profile>> GetProfilesForSearchAsync(int accountId, int profileBatchSize);

        Task<int> GetCountProfilesInProcessAsync(int accountId);

        Task<int> GetCountNewProfilesAsync(int accountId);

        Task InsertProfilesAsync(IEnumerable<Profile> profiles);

        Task UpdateProfilesAsync(IEnumerable<Profile> profiles);

        Task UpdateProfilesExecutionStatusByCompanyIdAsync(int accountId, int companyId, ExecutionStatuses executionStatus);
    }
}
