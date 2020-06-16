using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.SuitableProfileRepository.Interfaces
{
    public interface ISuitableProfileRepository
    {
        Task<SuitableProfile> GetSuitableProfileByIdAsync(int id);

        Task<SearchSuitableProfilesResponse> GetSuitableProfilesAsync(int accountId, SearchSuitablesProfilesRequest request);

        Task InsertSuitableProfilesAsync(IEnumerable<SuitableProfile> suitableProfiles);

        Task<ExportSuitablesProfilesResponse> ExportSuitablesProfilesAsync(int accountId, SearchSuitablesProfilesRequest request);
    }
}
