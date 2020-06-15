using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.ObjectMappers;
using ScraperLinkedInServer.Repositories.CompanyRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.CompanyRepository
{
    public class CompanyRepository : ICompanyRepository
    {
        public async Task<Company> GetCompanyByIdAsync(int id, int accountId)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                return await db.Companies.Where(x => x.Id == id && x.AccountId == accountId).FirstOrDefaultAsync();
            }
        }

        public async Task<SearchCompaniesResponse> SearchCompaniesAsync(int accountId, SearchCompaniesRequest request)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var response = new SearchCompaniesResponse();

                try
                {
                    var startDate = request.StartDate.Date;
                    var endDate = request.EndDate.Date;

                    var result = db.Companies.Where(x => x.AccountId == accountId
                    && (DbFunctions.TruncateTime(x.DateCreated) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(x.DateCreated) <= DbFunctions.TruncateTime(endDate)))
                        .AsQueryable();

                    if (!string.IsNullOrEmpty(request.SearchValue) && request.SearchValue.Length > 2)
                    {
                        result = result.Where(x => (x.OrganizationName.Contains(request.SearchValue) || x.Specialties.Contains(request.SearchValue)));
                    }

                    if(request.ExecutionStatus != Models.Types.ExecutionStatus.Any)
                    {
                        result = result.Where(x => x.ExecutionStatusID == (int)request.ExecutionStatus);
                    }

                    switch (request.SortedFieldType)
                    {
                        case SortedCompaniesFieldTypes.OrganizationName:
                            result = request.IsAscending
                                ? result.OrderBy(m => m.OrganizationName)
                                : result.OrderByDescending(m => m.OrganizationName);
                            break;
                        case SortedCompaniesFieldTypes.HeadquartersLocation:
                            result = request.IsAscending
                                ? result.OrderBy(m => m.HeadquartersLocation)
                                : result.OrderByDescending(m => m.HeadquartersLocation);
                            break;
                        case SortedCompaniesFieldTypes.Website:
                            result = request.IsAscending
                                ? result.OrderBy(m => m.Website)
                                : result.OrderByDescending(m => m.Website);
                            break;
                        case SortedCompaniesFieldTypes.Specialties:
                            result = request.IsAscending
                                ? result.OrderBy(m => m.Specialties)
                                : result.OrderByDescending(m => m.Specialties);
                            break;
                        case SortedCompaniesFieldTypes.AmountEmployees:
                            result = request.IsAscending
                                ? result.OrderBy(m => m.AmountEmployees)
                                : result.OrderByDescending(m => m.AmountEmployees);
                            break;
                        case SortedCompaniesFieldTypes.ExecutionStatus:
                            result = request.IsAscending
                                ? result.OrderBy(m => m.ExecutionStatusID)
                                : result.OrderByDescending(m => m.ExecutionStatusID);
                            break;
                        case SortedCompaniesFieldTypes.DateCreated:
                        default:
                            result = request.IsAscending
                                ? result.OrderBy(m => m.DateCreated)
                                : result.OrderByDescending(m => m.DateCreated);
                            break;
                    }

                    response.TotalCount = await result.CountAsync();

                    if (request.PageNumber >= 0 && request.PageSize > 0)
                    {
                        result = result.Skip(request.PageSize * (request.PageNumber - 1)).Take(request.PageSize);
                    }

                    response.SearchCompaniesViewModel = Mapper.Instance.Map<IEnumerable<Company>, IEnumerable<SearchCompaniesViewModel>>(await result.ToListAsync());
                }
                catch (Exception ex)
                {
                    response.ErrorMessage = ex.Message;
                    response.StatusCode = 400;
                }

                return response;
            }
        }

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

        public async Task<IEnumerable<Company>> GetCompaniesProfilesForSearchAsync(int accountId, int companyBatchSize)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                //var lastProcessedCompanyId = db.Profiles.Where(x => x.Company.AccountId == accountId && (x.ExecutionStatusID == (int)Models.Types.ExecutionStatus.Queued) && (x.ProfileStatusID != (int)Models.Types.ProfileStatus.Undefined))
                //                                        .OrderByDescending(d => d.Id)
                //                                        .Select(x => x.CompanyID)
                //                                        .FirstOrDefault();

                //var unsuitableCompanies = await db.Companies.Where(x => x.AccountId == accountId && (x.Id < lastProcessedCompanyId) && (x.ExecutionStatusID == (int)Models.Types.ExecutionStatus.Success) && x.Profiles.Any(y => (y.ExecutionStatusID != (int)Models.Types.ExecutionStatus.Success)) && !x.Profiles.Any(y => (y.ProfileStatusID == (int)Models.Types.ProfileStatus.Developer)))
                //                                      .ToListAsync();
                //unsuitableCompanies.ForEach(x => x.Profiles.ToList().ForEach(y => y.ExecutionStatusID = (int)Models.Types.ExecutionStatus.Success));
                //await db.SaveChangesAsync();
                return await db.Companies.Where(x => x.AccountId == accountId && (x.ExecutionStatusID == (int)Models.Types.ExecutionStatus.Success) && x.Profiles.Any() && !x.Profiles.Any(y => (y.ProfileStatusID == (int)Models.Types.ProfileStatus.Undefined) || (y.ExecutionStatusID != (int)Models.Types.ExecutionStatus.Queued)))
                                         .Include(x => x.Profiles)
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

                company.DateCreated = DateTime.UtcNow;

                db.Companies.Add(company);
                await db.SaveChangesAsync();
            }
        }

        public async Task<int> InsertCompaniesAsync(int accountId, IEnumerable<Company> companies)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                var addedCompaniesUrl = await db.Companies.Where(x => x.LinkedInURL != null && x.LinkedInURL.Trim() != "" && x.ExecutionStatusID != (int)Models.Types.ExecutionStatus.Success)
                                                    .Select(x => x.LinkedInURL).ToListAsync();
                companies = companies.Where(x => !addedCompaniesUrl.Contains(x.LinkedInURL));

                foreach (var company in companies)
                {
                    company.ExecutionStatusID = (int)Models.Types.ExecutionStatus.Created;
                    company.AccountId = accountId;
                    company.Founders = company.Founders ?? "";
                    company.Website = company.Website ?? "";
                    company.DateCreated = DateTime.UtcNow.Date;
                    company.Specialties = company.Specialties ?? "";
                    company.LogoUrl = company.LogoUrl ?? "";

                    db.Companies.Add(company);
                }

                db.SaveChanges();
                await db.SaveChangesAsync();

                return companies.Count();
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