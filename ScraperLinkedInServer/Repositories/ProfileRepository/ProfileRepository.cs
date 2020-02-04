using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Repositories.ProfileRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.ProfileRepository
{
    public class ProfileRepository : IProfileRepository
    {
        public async Task<IEnumerable<Profile>> GetProfilesAsync(int accountId, int profileBatchSize)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var result = db.Profiles.Where(x => x.AccountID == accountId && ((x.ProfileStatusID == (int)ProfileStatuses.Undefined) && (x.ExecutionStatusID == (int)ExecutionStatuses.Created || x.ExecutionStatusID == (int)ExecutionStatuses.Queued)))
                                        .Take(profileBatchSize);

                result.Where(x => x.ExecutionStatusID == (int)ExecutionStatuses.Created)
                      .ToList()
                      .ForEach(x => x.ExecutionStatusID = (int)ExecutionStatuses.Queued);

                await db.SaveChangesAsync();

                return result;
            }
        }

        public async Task<int> CountProfilesInProcessAsync(int accountId)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.Profiles.Where(x => x.Company.AccountId == accountId && (((x.ExecutionStatusID == (int)ExecutionStatuses.Created) || (x.ExecutionStatusID == (int)ExecutionStatuses.Queued)) && (x.ProfileStatusID == (int)ProfileStatuses.Undefined)))
                                        .CountAsync();
            }
        }

        public async Task<int> GetCountRawProfilesAsync(int accountId)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.Profiles.Where(x => x.AccountID == accountId && ((x.ProfileStatusID == (int)ProfileStatuses.Undefined) && (x.ExecutionStatusID == (int)ExecutionStatuses.Created || x.ExecutionStatusID == (int)ExecutionStatuses.Queued)))
                                  .CountAsync();
            }
        }

        public async Task<int> GetCountNewProfilesAsync(int accountId)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.Profiles.Where(x => x.AccountID == accountId && (DbFunctions.TruncateTime(x.DateСreation) == DbFunctions.TruncateTime(DateTime.Now)) && (x.ProfileStatusID == (int)ProfileStatuses.Undefined) && (x.ExecutionStatusID == (int)ExecutionStatuses.Created))
                                        .CountAsync();
            }
        }

        public async Task AddProfilesAsync(IEnumerable<Profile> profiles)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var companyID = profiles.FirstOrDefault().CompanyID;
                var addProfiles = db.Profiles.Where(x => x.CompanyID == companyID)
                                             .Select(x => x.ProfileUrl);

                foreach (var profile in profiles.Where(x => !addProfiles.Contains(x.ProfileUrl)))
                {
                    profile.DateСreation = DateTime.Now;
                    db.Profiles.Add(profile);
                }

                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateProfileAsync(Profile profile)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var profileDB = db.Profiles.Where(x => x.Id == profile.Id).FirstOrDefault();

                profileDB.FirstName = profile.FirstName ?? "";
                profileDB.LastName = profile.LastName ?? "";
                profileDB.FullName = profile.FullName ?? "";
                profileDB.Job = profile.Job ?? "";
                profileDB.AllSkills = profile.AllSkills ?? "";
                profileDB.ExecutionStatusID = profile.ExecutionStatusID;
                profileDB.ProfileStatusID = profile.ProfileStatusID;
                profileDB.DateСreation = profile.DateСreation;

                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateProfilesExecutionStatusByCompanyIdAsync(int accountId, int companyId, ExecutionStatuses executionStatus)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                db.Profiles.Where(x => x.AccountID == accountId && x.CompanyID == companyId)
                           .ToList()
                           .ForEach(y => y.ExecutionStatusID = (int)executionStatus);

                await db.SaveChangesAsync();
            }
        }
    }
}