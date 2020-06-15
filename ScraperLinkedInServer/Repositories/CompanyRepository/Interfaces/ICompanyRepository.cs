using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.CompanyRepository.Interfaces
{
    public interface ICompanyRepository
    {
        Task<SearchCompaniesResponse> SearchCompaniesAsync(int accountId, SearchCompaniesRequest request);

        Task<IEnumerable<Company>> GetCompaniesForSearchAsync(int accountId, int companyBatchSize);

        Task<IEnumerable<Company>> GetCompaniesProfilesForSearchAsync(int accountId, int companyBatchSize);

        Task<int> GetCountCompaniesInProcess(int accountId);

        Task InsertCompanyAsync(Company company);

        Task<int> InsertCompaniesAsync(int accountId, IEnumerable<Company> companies);

        Task UpdateCompanyAsync(Company company);

        Task UpdateCompaniesAsync(IEnumerable<Company> companies);

        Task<Company> GetCompanyByIdAsync(int id, int accountId);

        Task UpdateLastPageCompanyAsync(int accountId, int companyId, int lastScrapedPage);
    }
}
