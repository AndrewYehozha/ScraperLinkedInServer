using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Repositories.SuitableProfileRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.SuitableProfileRepository
{
    public class SuitableProfileRepository : ISuitableProfileRepository
    {
        public async Task<SuitableProfile> GetSuitableProfileByIdAsync(int id)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.SuitableProfiles.Where(x => x.Id == id).FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<SuitableProfile>> GetSuitableProfilesAsync(DateTime startDate, DateTime endDate, int accountId, int page = 1, int size = 50)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.SuitableProfiles.Where(x => x.AccountID == accountId && x.DateTimeCreation >= startDate && x.DateTimeCreation <= endDate)
                                                .OrderByDescending(x => x.DateTimeCreation)
                                                .Skip((page * size) - size).Take(size)
                                                .ToListAsync();
            }
        }

        public async Task InsertSuitableProfilesAsync(IEnumerable<SuitableProfile> suitableProfiles)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                foreach (var suitableProfile in suitableProfiles)
                {
                    suitableProfile.FirstName = string.IsNullOrEmpty(suitableProfile.FirstName) ? "..." : suitableProfile.FirstName;
                    suitableProfile.LastName = string.IsNullOrEmpty(suitableProfile.LastName) ? "..." : suitableProfile.LastName;
                    suitableProfile.Job = string.IsNullOrEmpty(suitableProfile.Job) ? "..." : suitableProfile.Job;
                    suitableProfile.PersonLinkedIn = string.IsNullOrEmpty(suitableProfile.PersonLinkedIn) ? "..." : suitableProfile.PersonLinkedIn;
                    suitableProfile.Company = string.IsNullOrEmpty(suitableProfile.Company) ? "..." : suitableProfile.Company;
                    suitableProfile.Website = string.IsNullOrEmpty(suitableProfile.Website) ? "..." : suitableProfile.Website;
                    suitableProfile.CompanyLogoUrl = string.IsNullOrEmpty(suitableProfile.CompanyLogoUrl) ? "..." : suitableProfile.CompanyLogoUrl;
                    suitableProfile.CrunchUrl = string.IsNullOrEmpty(suitableProfile.CrunchUrl) ? "..." : suitableProfile.CrunchUrl;
                    suitableProfile.Email = string.IsNullOrEmpty(suitableProfile.Email) ? "..." : suitableProfile.Email;
                    suitableProfile.EmailStatus = string.IsNullOrEmpty(suitableProfile.EmailStatus) ? "..." : suitableProfile.EmailStatus;
                    suitableProfile.City = string.IsNullOrEmpty(suitableProfile.City) ? "..." : suitableProfile.City;
                    suitableProfile.State = string.IsNullOrEmpty(suitableProfile.State) ? "..." : suitableProfile.State;
                    suitableProfile.Country = string.IsNullOrEmpty(suitableProfile.Country) ? "..." : suitableProfile.Country;
                    suitableProfile.PhoneNumber = string.IsNullOrEmpty(suitableProfile.PhoneNumber) ? "..." : suitableProfile.PhoneNumber;
                    suitableProfile.AmountEmployees = string.IsNullOrEmpty(suitableProfile.AmountEmployees) ? "..." : suitableProfile.AmountEmployees;
                    suitableProfile.Industry = string.IsNullOrEmpty(suitableProfile.Industry) ? "..." : suitableProfile.Industry;
                    suitableProfile.Twitter = string.IsNullOrEmpty(suitableProfile.Twitter) ? "..." : suitableProfile.Twitter;
                    suitableProfile.Facebook = string.IsNullOrEmpty(suitableProfile.Facebook) ? "..." : suitableProfile.Facebook;
                    suitableProfile.TechStack = string.IsNullOrEmpty(suitableProfile.TechStack) ? "..." : suitableProfile.TechStack;
                    suitableProfile.DateTimeCreation = DateTime.Now;

                    db.SuitableProfiles.Add(suitableProfile);
                }

                await db.SaveChangesAsync();
            }
        }
    }
}