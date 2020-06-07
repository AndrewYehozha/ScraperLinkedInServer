using ScraperLinkedInServer.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.CompanyRepository.Interfaces
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetCompaniesForSearchAsync(int accountId, int companyBatchSize);

        Task<IEnumerable<Company>> GetCompaniesProfilesForSearchAsync(int accountId, int companyBatchSize);

        Task<int> GetCountCompaniesInProcess(int accountId);

        Task InsertCompanyAsync(Company company);

        Task<int> InsertCompaniesAsync(int accountId, IEnumerable<Company> companies);

        Task UpdateCompanyAsync(Company company);

        Task UpdateCompaniesAsync(IEnumerable<Company> companies);

        Task UpdateLastPageCompanyAsync(int accountId, int companyId, int lastScrapedPage);
    }
}
