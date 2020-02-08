﻿using ScraperLinkedInServer.Extensions;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Services.ProfileService.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/v1/profiles")]
    public class ProfilesV1Controller : ScraperLinkedInApiController
    {
        private readonly IProfileService profileService;

        public ProfilesV1Controller(
            IProfileService profileService)
        {
            this.profileService = profileService;
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IHttpActionResult> GetProfileByIdAsync(int id)
        {
            var response = new ProfileResponse();

            var accountId = Identity.ToAccountID();
            var profile = await profileService.GetProfileByIdAsync(id, accountId);
            response.ProfileViewModel = profile;

            return JsonSuccess(response);
        }

        [HttpGet]
        [Route("windows-service-scraper")]
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> GetProfilesForSearchAsync(int profileBatchSize)
        {
            var response = new ProfilesResponse();

            var accountId = Identity.ToAccountID();
            var profiles = await profileService.GetProfilesForSearchAsync(accountId, profileBatchSize);
            var countProfilesInProcess = await profileService.GetCountProfilesInProcessAsync(accountId);
            var countNewProfiles = await profileService.GetCountNewProfilesAsync(accountId);
            response.ProfilesViewModel = profiles;
            response.CountProfilesInProcess = countProfilesInProcess;
            response.CountNewProfiles = countNewProfiles;

            return JsonSuccess(response);
        }

        [HttpGet]
        [Route("in-process/count")]
        [Authorize]
        public async Task<IHttpActionResult> GetCountProfilesInProcessAsync()
        {
            var response = new ProfilesResponse();

            var accountId = Identity.ToAccountID();
            var countProfilesInProcess = await profileService.GetCountProfilesInProcessAsync(accountId);
            response.CountProfilesInProcess = countProfilesInProcess;

            return JsonSuccess(response);
        }

        [HttpGet]
        [Route("new/count")]
        [Authorize]
        public async Task<IHttpActionResult> GetCountNewProfilesAsync()
        {
            var response = new ProfilesResponse();

            var accountId = Identity.ToAccountID();
            var countNewProfiles = await profileService.GetCountNewProfilesAsync(accountId);
            response.CountNewProfiles = countNewProfiles;

            return JsonSuccess(response);
        }

        [HttpPost]
        [Route("windows-service-scraper")]
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> InsertProfilesAsync(ProfilesRequest request)
        {
            var response = new ProfilesResponse();

            await profileService.InsertProfilesAsync(request.ProfilesViewModel); ;

            return JsonSuccess(response);
        }

        [HttpPut]
        [Route("windows-service-scraper")]
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> UpdateProfilesAsync(ProfilesRequest request)
        {
            var response = new ProfilesResponse();

            await profileService.UpdateProfilesAsync(request.ProfilesViewModel);

            return JsonSuccess(response);
        }
    }
}
