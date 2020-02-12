using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.ObjectMappers;
using ScraperLinkedInServer.Repositories.ProfileRepository.Interfaces;
using ScraperLinkedInServer.Services.ProfileService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.ProfileService
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileService(
            IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<ProfileViewModel> GetProfileByIdAsync(int id, int accountId)
        {
            var profileDb = await _profileRepository.GetProfileByIdAsync(id, accountId);
            return Mapper.Instance.Map<Profile, ProfileViewModel>(profileDb);
        }

        public async Task<IEnumerable<ProfileViewModel>> GetProfilesForSearchAsync(int accountId, int profileBatchSize)
        {
            var profilesDb = await _profileRepository.GetProfilesForSearchAsync(accountId, profileBatchSize);
            return Mapper.Instance.Map<IEnumerable<Profile>, IEnumerable<ProfileViewModel>>(profilesDb);
        }

        public async Task<int> GetCountProfilesInProcessAsync(int accountId)
        {
            return await _profileRepository.GetCountProfilesInProcessAsync(accountId);
        }

        public async Task<int> GetCountNewProfilesAsync(int accountId)
        {
            return await _profileRepository.GetCountNewProfilesAsync(accountId);
        }

        public async Task InsertProfilesAsync(IEnumerable<ProfileViewModel> profilesVM)
        {
            var profilesDb = Mapper.Instance.Map<IEnumerable<ProfileViewModel>, IEnumerable<Profile>>(profilesVM);
            await _profileRepository.InsertProfilesAsync(profilesDb);
        }

        public async Task UpdateProfilesAsync(IEnumerable<ProfileViewModel> profilesVM)
        {
            var profilesDb = Mapper.Instance.Map<IEnumerable<ProfileViewModel>, IEnumerable<Profile>>(profilesVM);
            await _profileRepository.UpdateProfilesAsync(profilesDb);
        }

        public async Task UpdateProfilesExecutionStatusByCompanyIdAsync(int accountId, int companyId, ExecutionStatuses executionStatus)
        {
            await _profileRepository.UpdateProfilesExecutionStatusByCompanyIdAsync(accountId, companyId, executionStatus);
        }
    }
}