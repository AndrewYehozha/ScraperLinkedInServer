﻿using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.ObjectMappers;
using ScraperLinkedInServer.Repositories.SuitableProfileRepository.Interfaces;
using ScraperLinkedInServer.Services.ProfileService.Interfaces;
using ScraperLinkedInServer.Services.SuitableProfileService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScraperLinkedInServer.Services.SuitableProfileService
{
    public class SuitableProfileService : ISuitableProfileService
    {
        private readonly ISuitableProfileRepository suitableProfileRepository;
        private readonly IProfileService profileService;

        public SuitableProfileService(
            ISuitableProfileRepository suitableProfileRepository,
            IProfileService profileService)
        {
            this.suitableProfileRepository = suitableProfileRepository;
            this.profileService = profileService;
        }

        public async Task<SuitableProfileViewModel> GetSuitableProfileByIdAsync(int id)
        {
            var suitableProfileDB = await suitableProfileRepository.GetSuitableProfileByIdAsync(id);
            return Mapper.Instance.Map<SuitableProfile, SuitableProfileViewModel>(suitableProfileDB);
        }

        public async Task<IEnumerable<SuitableProfileViewModel>> GetSuitableProfilesAsync(DateTime startDate, DateTime endDate, int accountId, int page, int size)
        {
            var suitableProfilesDB = await suitableProfileRepository.GetSuitableProfilesAsync(startDate, endDate, accountId, page, size);
            return Mapper.Instance.Map<IEnumerable<SuitableProfile>, IEnumerable<SuitableProfileViewModel>>(suitableProfilesDB);
        }

        [HttpPost]
        [Route("suitable-profile-management")]
        [Authorize]
        public async Task InsertSuitableProfilesAsync(IEnumerable<SuitableProfileViewModel> suitableProfilesVM)
        {
            var suitableProfilesDB = Mapper.Instance.Map<IEnumerable<SuitableProfileViewModel>, IEnumerable<SuitableProfile>>(suitableProfilesVM);
            await suitableProfileRepository.InsertSuitableProfilesAsync(suitableProfilesDB);

            var companiesIds = suitableProfilesVM.Select(x => x.CompanyID).Distinct();
            foreach (var companyId in companiesIds)
            {
                var accountId = suitableProfilesVM.Where(x => x.CompanyID == companyId).FirstOrDefault().AccountID;
                await profileService.UpdateProfilesExecutionStatusByCompanyIdAsync(accountId, companyId, ExecutionStatuses.Success);
            }
        }
    }
}