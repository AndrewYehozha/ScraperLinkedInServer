using ScraperLinkedInServer.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.SuitableProfileRepository.Interfaces
{
    public interface ISuitableProfileRepository
    {
        Task<SuitableProfile> GetSuitableProfileByIdAsync(int id);

        Task<IEnumerable<SuitableProfile>> GetSuitableProfilesAsync(DateTime startDate, DateTime endDate, int accountId, int page, int size);

        Task InsertSuitableProfilesAsync(IEnumerable<SuitableProfile> suitableProfiles);
    }
}
