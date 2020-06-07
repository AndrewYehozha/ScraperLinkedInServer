using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.CompanyService.Interfaces
{
    public interface ICompanyService
    {
        Task<SearchCompaniesResponse> SearchCompaniesAsync(int accountId, SearchCompaniesRequest request);

        Task<IEnumerable<CompanyViewModel>> GetCompaniesForSearchAsync(int accountId, int companyBatchSize);

        Task<IEnumerable<CompanyProfilesViewModel>> GetCompaniesProfilesForSearchAsync(int accountId, int companyBatchSize);

        Task<int> GetCountCompaniesInProcess(int accountId);

        Task InsertCompanyAsync(CompanyViewModel companyVM);

        Task<int> InsertCompaniesAsync(int accountId, IEnumerable<CompanyViewModel> companiesVM);

        Task UpdateCompanyAsync(CompanyViewModel companyVM);

        Task UpdateCompaniesAsync(IEnumerable<CompanyViewModel> companiesVM);

        Task UpdateLastPageCompanyAsync(int accountId, int companyId, int lastScrapedPage);
    }
}
