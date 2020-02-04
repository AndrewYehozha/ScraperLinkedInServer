using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.ProfileRepository.Interfaces
{
    public interface IProfileRepository
    {
        Task<IEnumerable<Profile>> GetProfilesAsync(int accountId, int profileBatchSize);

        Task<int> CountProfilesInProcessAsync(int accountId);

        Task<int> GetCountRawProfilesAsync(int accountId);

        Task<int> GetCountNewProfilesAsync(int accountId);

        Task AddProfilesAsync(IEnumerable<Profile> profiles);

        Task UpdateProfileAsync(Profile profile);

        Task UpdateProfilesExecutionStatusByCompanyIdAsync(int accountId, int companyId, ExecutionStatuses executionStatus);
    }
}
