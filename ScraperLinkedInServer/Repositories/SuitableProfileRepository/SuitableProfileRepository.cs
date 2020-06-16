using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.ObjectMappers;
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

        public async Task<SearchSuitableProfilesResponse> GetSuitableProfilesAsync(int accountId, SearchSuitablesProfilesRequest request)
        {
            var response = new SearchSuitableProfilesResponse();

            try
            {
                using (var db = new ScraperLinkedInDBEntities())
                {
                    var companiesSearchResult = SearchSuitableProfilesAsIQueryable(db, accountId, request);

                    response.TotalCount = await companiesSearchResult.CountAsync();

                    if (request.PageNumber >= 0 && request.PageSize > 0)
                    {
                        companiesSearchResult = companiesSearchResult.Skip(request.PageSize * (request.PageNumber - 1)).Take(request.PageSize);
                    }

                    response.SearchSuitableProfilesViewModel = Mapper.Instance.Map<IEnumerable<SuitableProfile>, IEnumerable<SearchSuitableProfilesViewModel>>(await companiesSearchResult.ToListAsync());
                    response.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.StatusCode = 400;
            }

            return response;
        }

        public async Task<ExportSuitablesProfilesResponse> ExportSuitablesProfilesAsync(int accountId, SearchSuitablesProfilesRequest request)
        {
            var response = new ExportSuitablesProfilesResponse();

            using (var db = new ScraperLinkedInDBEntities())
            {
                var suitableProfilesSearchResult = SearchSuitableProfilesAsIQueryable(db, accountId, request);
                response.SuitablesProfilesEntriesCount = await suitableProfilesSearchResult.CountAsync();
                response.ExportSuitableProfilesViewModel = Mapper.Instance.Map<IEnumerable<SuitableProfile>, IEnumerable<SearchSuitableProfilesViewModel>>(await suitableProfilesSearchResult.ToListAsync());
            }

            return response;
        }

        private IQueryable<SuitableProfile> SearchSuitableProfilesAsIQueryable(ScraperLinkedInDBEntities db, int accountId, SearchSuitablesProfilesRequest request)
        {
            var startDate = request.StartDate.Date;
            var endDate = request.EndDate.Date;

            var result = db.SuitableProfiles.Where(x => x.AccountID == accountId
            && (DbFunctions.TruncateTime(x.DateTimeCreation) >= DbFunctions.TruncateTime(startDate) && DbFunctions.TruncateTime(x.DateTimeCreation) <= DbFunctions.TruncateTime(endDate)))
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchValue) && request.SearchValue.Length > 2)
            {
                result = result.Where(x => (x.FirstName.Contains(request.SearchValue) || x.LastName.Contains(request.SearchValue) || x.Company.Contains(request.SearchValue)));
            }

            if (request.ProfileStatus != Models.Types.ProfileStatus.Any)
            {
                result = result.Where(x => x.ProfileStatusID == (int)request.ProfileStatus);
            }

            if (request.CompanyId > 0)
            {
                result = result.Where(x => x.CompanyID == request.CompanyId);
            }

            switch (request.SortedFieldType)
            {
                case SortedSuitablesProfilesFieldTypes.FirstName:
                    result = request.IsAscending
                        ? result.OrderBy(m => m.FirstName)
                        : result.OrderByDescending(m => m.FirstName);
                    break;
                case SortedSuitablesProfilesFieldTypes.LastName:
                    result = request.IsAscending
                        ? result.OrderBy(m => m.LastName)
                        : result.OrderByDescending(m => m.LastName);
                    break;
                case SortedSuitablesProfilesFieldTypes.Job:
                    result = request.IsAscending
                        ? result.OrderBy(m => m.Job)
                        : result.OrderByDescending(m => m.Job);
                    break;
                case SortedSuitablesProfilesFieldTypes.PersonLinkedIn:
                    result = request.IsAscending
                        ? result.OrderBy(m => m.PersonLinkedIn)
                        : result.OrderByDescending(m => m.PersonLinkedIn);
                    break;
                case SortedSuitablesProfilesFieldTypes.Company:
                    result = request.IsAscending
                        ? result.OrderBy(m => m.Company)
                        : result.OrderByDescending(m => m.Company);
                    break;
                case SortedSuitablesProfilesFieldTypes.Email:
                    result = request.IsAscending
                        ? result.OrderBy(m => m.Email)
                        : result.OrderByDescending(m => m.Email);
                    break;
                case SortedSuitablesProfilesFieldTypes.TechStack:
                    result = request.IsAscending
                        ? result.OrderBy(m => m.TechStack)
                        : result.OrderByDescending(m => m.TechStack);
                    break;
                case SortedSuitablesProfilesFieldTypes.ProfileStatus:
                    result = request.IsAscending
                        ? result.OrderBy(m => m.ProfileStatusID)
                        : result.OrderByDescending(m => m.ProfileStatusID);
                    break;
                case SortedSuitablesProfilesFieldTypes.DateTimeCreation:
                default:
                    result = request.IsAscending
                        ? result.OrderBy(m => m.DateTimeCreation)
                        : result.OrderByDescending(m => m.DateTimeCreation);
                    break;
            }

            return result;
        }

        public async Task InsertSuitableProfilesAsync(IEnumerable<SuitableProfile> suitableProfiles)
        {
            using (var db = new ScraperLinkedInDBEntities())
            {
                foreach (var suitableProfile in suitableProfiles)
                {
                    suitableProfile.FirstName = string.IsNullOrEmpty(suitableProfile.FirstName) ? "" : suitableProfile.FirstName;
                    suitableProfile.LastName = string.IsNullOrEmpty(suitableProfile.LastName) ? "" : suitableProfile.LastName;
                    suitableProfile.Job = string.IsNullOrEmpty(suitableProfile.Job) ? "" : suitableProfile.Job;
                    suitableProfile.PersonLinkedIn = string.IsNullOrEmpty(suitableProfile.PersonLinkedIn) ? "" : suitableProfile.PersonLinkedIn;
                    suitableProfile.Company = string.IsNullOrEmpty(suitableProfile.Company) ? "" : suitableProfile.Company;
                    suitableProfile.Website = string.IsNullOrEmpty(suitableProfile.Website) ? "" : suitableProfile.Website;
                    suitableProfile.CompanyLogoUrl = string.IsNullOrEmpty(suitableProfile.CompanyLogoUrl) ? "" : suitableProfile.CompanyLogoUrl;
                    suitableProfile.CrunchUrl = string.IsNullOrEmpty(suitableProfile.CrunchUrl) ? "" : suitableProfile.CrunchUrl;
                    suitableProfile.Email = string.IsNullOrEmpty(suitableProfile.Email) ? "" : suitableProfile.Email;
                    suitableProfile.EmailStatus = string.IsNullOrEmpty(suitableProfile.EmailStatus) ? "" : suitableProfile.EmailStatus;
                    suitableProfile.City = string.IsNullOrEmpty(suitableProfile.City) ? "" : suitableProfile.City;
                    suitableProfile.State = string.IsNullOrEmpty(suitableProfile.State) ? "" : suitableProfile.State;
                    suitableProfile.Country = string.IsNullOrEmpty(suitableProfile.Country) ? "" : suitableProfile.Country;
                    suitableProfile.PhoneNumber = string.IsNullOrEmpty(suitableProfile.PhoneNumber) ? "" : suitableProfile.PhoneNumber;
                    suitableProfile.AmountEmployees = string.IsNullOrEmpty(suitableProfile.AmountEmployees) ? "" : suitableProfile.AmountEmployees;
                    suitableProfile.Industry = string.IsNullOrEmpty(suitableProfile.Industry) ? "" : suitableProfile.Industry;
                    suitableProfile.Twitter = string.IsNullOrEmpty(suitableProfile.Twitter) ? "" : suitableProfile.Twitter;
                    suitableProfile.Facebook = string.IsNullOrEmpty(suitableProfile.Facebook) ? "" : suitableProfile.Facebook;
                    suitableProfile.TechStack = string.IsNullOrEmpty(suitableProfile.TechStack) ? "" : suitableProfile.TechStack;
                    suitableProfile.DateTimeCreation = DateTime.Now;

                    db.SuitableProfiles.Add(suitableProfile);
                }

                await db.SaveChangesAsync();
            }
        }
    }
}