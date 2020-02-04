using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Repositories.CompanyRepository.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.CompanyRepository
{
    public class CompanyRepository : ICompanyRepository
    {
        public async Task<IEnumerable<Company>> GetCompaniesForSearchAsync(int accountId, int companyBatchSize)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var companiesDb = db.Companies.Where(x => x.AccountId == accountId && !string.IsNullOrEmpty(x.LinkedInURL.Trim()) && !string.IsNullOrEmpty(x.Website.Trim()) && (x.ExecutionStatusID == (int)ExecutionStatuses.Created || x.ExecutionStatusID == (int)ExecutionStatuses.Queued))
                                              .Take(companyBatchSize);

                companiesDb.ToList().ForEach(x => x.ExecutionStatusID = (int)ExecutionStatuses.Queued);
                await db.SaveChangesAsync();

                return companiesDb;
            }
        }

        public async Task<IEnumerable<Company>> GetCompaniesForSearchSuitableProfilesAsync(int accountId)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var lastProcessedCompanyId = db.Profiles.Where(x => x.Company.AccountId == accountId && (x.ExecutionStatusID == (int)ExecutionStatuses.Queued) && (x.ProfileStatusID != (int)ProfileStatuses.Undefined))
                                                        .OrderByDescending(d => d.Id)
                                                        .Select(x => x.CompanyID)
                                                        .FirstOrDefault();

                var unsuitableCompanies = await db.Companies.Where(x => x.AccountId == accountId && (x.Id < lastProcessedCompanyId) && (x.ExecutionStatusID == (int)ExecutionStatuses.Success) && x.Profiles.Any(y => (y.ExecutionStatusID != (int)ExecutionStatuses.Success)) && !x.Profiles.Any(y => (y.ProfileStatusID == (int)ProfileStatuses.Developer)))
                                                      .ToListAsync();
                unsuitableCompanies.ForEach(x => x.Profiles.ToList().ForEach(y => y.ExecutionStatusID = (int)ExecutionStatuses.Success));
                await db.SaveChangesAsync();

                return await db.Companies.Where(x => x.AccountId == accountId && (x.Id < lastProcessedCompanyId) && (x.ExecutionStatusID == (int)ExecutionStatuses.Success) && x.Profiles.Any(y => (y.ProfileStatusID == (int)ProfileStatuses.Developer) && (y.ExecutionStatusID != (int)ExecutionStatuses.Success)))
                                         .ToListAsync();
            }
        }

        public async Task<int> CountCompaniesInProcess(int accountId)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.Companies.Where(x => x.AccountId == accountId && ((x.ExecutionStatusID == (int)ExecutionStatuses.Created) || (x.ExecutionStatusID == (int)ExecutionStatuses.Queued)))
                                         .CountAsync();
            }
        }

        public async Task InsertCompanyAsync(Company company)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                db.Companies.Add(company);
                await db.SaveChangesAsync();
            }
        }

        public async Task InsertCompaniesAsync(IEnumerable<Company> companies)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                foreach (var company in companies)
                {
                    db.Companies.Add(company);
                }

                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateCompanyAsync(Company company)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var companyDB = await db.Companies.Where(x => x.Id == company.Id).FirstOrDefaultAsync();

                companyDB.LogoUrl = company.LogoUrl ?? "";
                companyDB.Specialties = company.Specialties ?? "";
                companyDB.ExecutionStatusID = company.ExecutionStatusID;

                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateCompaniesAsync(IEnumerable<Company> companies)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                foreach (var company in companies)
                {
                    var companyDB = await db.Companies.Where(x => x.Id == company.Id).FirstOrDefaultAsync();

                    companyDB.LogoUrl = company.LogoUrl ?? "";
                    companyDB.Specialties = company.Specialties ?? "";
                    companyDB.ExecutionStatusID = company.ExecutionStatusID;
                }

                await db.SaveChangesAsync();
            }
        }
    }
}