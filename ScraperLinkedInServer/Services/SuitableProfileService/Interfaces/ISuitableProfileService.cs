using ScraperLinkedInServer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.SuitableProfileService.Interfaces
{
    public interface ISuitableProfileService
    {
        Task<SuitableProfileViewModel> GetSuitableProfileByIdAsync(int id);

        Task<IEnumerable<SuitableProfileViewModel>> GetSuitableProfilesAsync(DateTime startDate, DateTime endDate, int accountId, int page, int size);

        Task InsertSuitableProfilesAsync(IEnumerable<SuitableProfileViewModel> suitableProfilesVM);
    }
}
