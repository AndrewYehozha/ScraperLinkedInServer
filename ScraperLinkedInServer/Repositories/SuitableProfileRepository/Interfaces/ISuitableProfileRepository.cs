using ScraperLinkedInServer.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.SuitableProfileRepository.Interfaces
{
    public interface ISuitableProfileRepository
    {
        Task<IEnumerable<SuitableProfile>> GetSuitableProfileAsync(DateTime startDate, DateTime endDate, int accountId, int page, int size);


        Task InsertSuitableProfilesAsync(IEnumerable<SuitableProfile> suitableProfiles);
    }
}
