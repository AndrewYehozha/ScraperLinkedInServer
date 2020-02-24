using ScraperLinkedInServer.Database;
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
                var companiesDb = db.Companies.Where(x => x.AccountId == accountId && !string.IsNullOrEmpty(x.LinkedInURL.Trim()) && (x.ExecutionStatusID == (int)Models.Types.ExecutionStatus.Created || x.ExecutionStatusID == (int)Models.Types.ExecutionStatus.Queued))
                                              .Take(companyBatchSize);

                Enumerable.ToList(companiesDb).ForEach(x => x.ExecutionStatusID = (int)Models.Types.ExecutionStatus.Queued);
                await db.SaveChangesAsync();

                return await companiesDb.ToListAsync();
            }
        }

        public async Task<IEnumerable<Company>> GetCompaniesForSearchSuitableProfilesAsync(int accountId, int companyBatchSize)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var lastProcessedCompanyId = db.Profiles.Where(x => x.Company.AccountId == accountId && (x.ExecutionStatusID == (int)Models.Types.ExecutionStatus.Queued) && (x.ProfileStatusID != (int)Models.Types.ProfileStatus.Undefined))
                                                        .OrderByDescending(d => d.Id)
                                                        .Select(x => x.CompanyID)
                                                        .FirstOrDefault();

                var unsuitableCompanies = await db.Companies.Where(x => x.AccountId == accountId && (x.Id < lastProcessedCompanyId) && (x.ExecutionStatusID == (int)Models.Types.ExecutionStatus.Success) && x.Profiles.Any(y => (y.ExecutionStatusID != (int)Models.Types.ExecutionStatus.Success)) && !x.Profiles.Any(y => (y.ProfileStatusID == (int)Models.Types.ProfileStatus.Developer)))
                                                      .ToListAsync();
                unsuitableCompanies.ForEach(x => x.Profiles.ToList().ForEach(y => y.ExecutionStatusID = (int)Models.Types.ExecutionStatus.Success));
                await db.SaveChangesAsync();

                return await db.Companies.Where(x => x.AccountId == accountId && (x.Id < lastProcessedCompanyId) && (x.ExecutionStatusID == (int)Models.Types.ExecutionStatus.Success) && x.Profiles.Any(y => (y.ProfileStatusID == (int)Models.Types.ProfileStatus.Developer) && (y.ExecutionStatusID != (int)Models.Types.ExecutionStatus.Success)))
                                         .Take(companyBatchSize)
                                         .ToListAsync();
            }
        }

        public async Task<int> GetCountCompaniesInProcess(int accountId)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.Companies.Where(x => x.AccountId == accountId && ((x.ExecutionStatusID == (int)Models.Types.ExecutionStatus.Created) || (x.ExecutionStatusID == (int)Models.Types.ExecutionStatus.Queued)))
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
                var addedCompaniesUrl = await db.Companies.Where(x => x.LinkedInURL != null && x.LinkedInURL.Trim() != "" && x.ExecutionStatusID != (int)Models.Types.ExecutionStatus.Success)
                                                    .Select(x => x.LinkedInURL).ToListAsync();
                companies = companies.Where(x => !addedCompaniesUrl.Contains(x.LinkedInURL));

                foreach (var company in companies)
                {
                    company.Founders = company.Founders ?? "";
                    company.Website = company.Website ?? "";

                    db.Companies.Add(company);
                }

                db.SaveChanges();
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

        public async Task UpdateLastPageCompanyAsync(int accountId, int companyId, int lastScrapedPage)
        {

            using (var db = new ScraperLinkedInDBEntities())
            {
                var companyDB = await db.Companies.Where(x => x.AccountId == accountId && x.Id == companyId).FirstOrDefaultAsync();
                if (companyDB != null)
                {
                    companyDB.LastScrapedPage = lastScrapedPage;

                    await db.SaveChangesAsync();
                }
            }
        }
    }
}