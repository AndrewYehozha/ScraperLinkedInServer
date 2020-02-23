using ScraperLinkedInServer.Database;
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
        public async Task<Profile> GetProfileByIdAsync(int id, int accountId)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.Profiles.Where(x => x.AccountID == accountId && x.Id == id).FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<Profile>> GetProfilesForSearchAsync(int accountId, int profilesBatchSize)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var result = db.Profiles.Where(x => x.AccountID == accountId && ((x.ProfileStatusID == (int)Models.Types.ProfileStatus.Undefined) && (x.ExecutionStatusID == (int)Models.Types.ExecutionStatus.Created || x.ExecutionStatusID == (int)Models.Types.ExecutionStatus.Queued)))
                                        .Take(profilesBatchSize).Include(x => x.Company);

                result.Where(x => x.ExecutionStatusID == (int)Models.Types.ExecutionStatus.Created)
                      .ToList()
                      .ForEach(x => x.ExecutionStatusID = (int)Models.Types.ExecutionStatus.Queued);

                await db.SaveChangesAsync();

                return await result.ToListAsync();
            }
        }

        public async Task<int> GetCountProfilesInProcessAsync(int accountId)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.Profiles.Where(x => x.AccountID == accountId && (((x.ExecutionStatusID == (int)Models.Types.ExecutionStatus.Created) || (x.ExecutionStatusID == (int)Models.Types.ExecutionStatus.Queued)) && (x.ProfileStatusID == (int)Models.Types.ProfileStatus.Undefined)))
                                        .CountAsync();
            }
        }

        public async Task<int> GetCountNewProfilesAsync(int accountId)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.Profiles.Where(x => x.AccountID == accountId && (DbFunctions.TruncateTime(x.DateСreation) == DbFunctions.TruncateTime(DateTime.Now)) && (x.ProfileStatusID == (int)Models.Types.ProfileStatus.Undefined) && (x.ExecutionStatusID == (int)Models.Types.ExecutionStatus.Created))
                                        .CountAsync();
            }
        }

        public async Task InsertProfilesAsync(IEnumerable<Profile> profiles)
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

        public async Task UpdateProfilesAsync(IEnumerable<Profile> profiles)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                foreach (var profile in profiles)
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
                }

                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateProfilesExecutionStatusByCompanyIdAsync(int accountId, int companyId, Models.Types.ExecutionStatus executionStatus)
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