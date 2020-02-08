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
        private readonly IProfileRepository profileRepository;

        public ProfileService(
            IProfileRepository profileRepository)
        {
            this.profileRepository = profileRepository;
        }

        public async Task<ProfileViewModel> GetProfileByIdAsync(int id, int accountId)
        {
            var profileDb = await profileRepository.GetProfileByIdAsync(id, accountId);
            return Mapper.Instance.Map<Profile, ProfileViewModel>(profileDb);
        }

        public async Task<IEnumerable<ProfileViewModel>> GetProfilesForSearchAsync(int accountId, int profileBatchSize)
        {
            var profilesDb = await profileRepository.GetProfilesForSearchAsync(accountId, profileBatchSize);
            return Mapper.Instance.Map<IEnumerable<Profile>, IEnumerable<ProfileViewModel>>(profilesDb);
        }

        public async Task<int> GetCountProfilesInProcessAsync(int accountId)
        {
            return await profileRepository.GetCountProfilesInProcessAsync(accountId);
        }

        public async Task<int> GetCountNewProfilesAsync(int accountId)
        {
            return await profileRepository.GetCountNewProfilesAsync(accountId);
        }

        public async Task InsertProfilesAsync(IEnumerable<ProfileViewModel> profilesVM)
        {
            var profilesDb = Mapper.Instance.Map<IEnumerable<ProfileViewModel>, IEnumerable<Profile>>(profilesVM);
            await profileRepository.InsertProfilesAsync(profilesDb);
        }

        public async Task UpdateProfilesAsync(IEnumerable<ProfileViewModel> profilesVM)
        {
            var profilesDb = Mapper.Instance.Map<IEnumerable<ProfileViewModel>, IEnumerable<Profile>>(profilesVM);
            await profileRepository.UpdateProfilesAsync(profilesDb);
        }

        public async Task UpdateProfilesExecutionStatusByCompanyIdAsync(int accountId, int companyId, ExecutionStatuses executionStatus)
        {
            await profileRepository.UpdateProfilesExecutionStatusByCompanyIdAsync(accountId, companyId, executionStatus);
        }
    }
}