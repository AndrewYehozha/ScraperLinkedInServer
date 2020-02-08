using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.CompanyService.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyViewModel>> GetCompaniesForSearchAsync(int accountId, int companyBatchSize);

        Task<IEnumerable<CompanyViewModel>> GetCompaniesForSearchSuitableProfilesAsync(int accountId);

        Task<int> GetCountCompaniesInProcess(int accountId);

        Task InsertCompanyAsync(CompanyViewModel companyVM);

        Task InsertCompaniesAsync(IEnumerable<CompanyViewModel> companiesVM);

        Task UpdateCompanyAsync(CompanyViewModel companyVM);

        Task UpdateCompaniesAsync(IEnumerable<CompanyViewModel> companiesVM);
    }
}
