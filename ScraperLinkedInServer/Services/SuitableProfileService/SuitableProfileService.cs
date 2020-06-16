using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.ObjectMappers;
using ScraperLinkedInServer.Repositories.SuitableProfileRepository.Interfaces;
using ScraperLinkedInServer.Services.ProfileService.Interfaces;
using ScraperLinkedInServer.Services.SuitableProfileService.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.SuitableProfileService
{
    public class SuitableProfileService : ISuitableProfileService
    {
        private readonly ISuitableProfileRepository _suitableProfileRepository;
        private readonly IProfileService _profileService;

        public SuitableProfileService(
            ISuitableProfileRepository suitableProfileRepository,
            IProfileService profileService)
        {
            _suitableProfileRepository = suitableProfileRepository;
            _profileService = profileService;
        }

        public async Task<SuitableProfileViewModel> GetSuitableProfileByIdAsync(int id)
        {
            var suitableProfileDB = await _suitableProfileRepository.GetSuitableProfileByIdAsync(id);
            return Mapper.Instance.Map<SuitableProfile, SuitableProfileViewModel>(suitableProfileDB);
        }

        public async Task<SearchSuitableProfilesResponse> GetSuitableProfilesAsync(int accountId, SearchSuitablesProfilesRequest request)
        {
            return await _suitableProfileRepository.GetSuitableProfilesAsync(accountId, request);
        }

        public async Task<ExportSuitablesProfilesResponse> SearchExportSuitablesProfilesAsync(int accountId, SearchSuitablesProfilesRequest request)
        {
            return await _suitableProfileRepository.ExportSuitablesProfilesAsync(accountId, request);
        }

        public async Task InsertSuitableProfilesAsync(IEnumerable<SuitableProfileViewModel> suitableProfilesVM)
        {
            var suitableProfilesDB = Mapper.Instance.Map<IEnumerable<SuitableProfileViewModel>, IEnumerable<SuitableProfile>>(suitableProfilesVM);
            await _suitableProfileRepository.InsertSuitableProfilesAsync(suitableProfilesDB);

            var companiesIds = suitableProfilesVM.Select(x => x.CompanyID).Distinct();
            foreach (var companyId in companiesIds)
            {
                var accountId = suitableProfilesVM.Where(x => x.CompanyID == companyId).FirstOrDefault().AccountID;
                await _profileService.UpdateProfilesExecutionStatusByCompanyIdAsync(accountId, companyId, Models.Types.ExecutionStatus.Success);
            }
        }
    }
}