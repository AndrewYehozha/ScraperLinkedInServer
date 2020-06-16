using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.SuitableProfileService.Interfaces
{
    public interface ISuitableProfileService
    {
        Task<SuitableProfileViewModel> GetSuitableProfileByIdAsync(int id);

        Task InsertSuitableProfilesAsync(IEnumerable<SuitableProfileViewModel> suitableProfilesVM);

        Task<SearchSuitableProfilesResponse> GetSuitableProfilesAsync(int accountId, SearchSuitablesProfilesRequest request);

        Task<ExportSuitablesProfilesResponse> SearchExportSuitablesProfilesAsync(int accountId, SearchSuitablesProfilesRequest request);
    }
}
