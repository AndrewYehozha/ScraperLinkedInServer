using ScraperLinkedInServer.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Repositories.CompanyRepository.Interfaces
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetCompaniesForSearchAsync(int accountId, int companyBatchSize);

        Task<IEnumerable<Company>> GetCompaniesForSearchSuitableProfilesAsync(int accountId, int companyBatchSize);

        Task<int> GetCountCompaniesInProcess(int accountId);

        Task InsertCompanyAsync(Company company);

        Task InsertCompaniesAsync(IEnumerable<Company> companies);

        Task UpdateCompanyAsync(Company company);

        Task UpdateCompaniesAsync(IEnumerable<Company> companies);
    }
}
