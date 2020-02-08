using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.ObjectMappers;
using ScraperLinkedInServer.Repositories.CompanyRepository.Interfaces;
using ScraperLinkedInServer.Services.CompanyService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.CompanyService
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository companyRepository;

        public CompanyService(
            ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        public async Task<IEnumerable<CompanyViewModel>> GetCompaniesForSearchAsync(int accountId, int companyBatchSize)
        {
            var companiesDb = await companyRepository.GetCompaniesForSearchAsync(accountId, companyBatchSize);
            return Mapper.Instance.Map<IEnumerable<Company>, IEnumerable<CompanyViewModel>>(companiesDb);
        }

        public async Task<IEnumerable<CompanyViewModel>> GetCompaniesForSearchSuitableProfilesAsync(int accountId)
        {
            var companiesDb = await companyRepository.GetCompaniesForSearchSuitableProfilesAsync(accountId);
            return Mapper.Instance.Map<IEnumerable<Company>, IEnumerable<CompanyViewModel>>(companiesDb);
        }

        public async Task<int> GetCountCompaniesInProcess(int accountId)
        {
            return await companyRepository.GetCountCompaniesInProcess(accountId);
        }

        public async Task InsertCompanyAsync(CompanyViewModel companyVM)
        {
            var companieDb = Mapper.Instance.Map<CompanyViewModel, Company>(companyVM);
            await companyRepository.InsertCompanyAsync(companieDb);
        }

        public async Task InsertCompaniesAsync(IEnumerable<CompanyViewModel> companiesVM)
        {
            var companiesDb = Mapper.Instance.Map<IEnumerable<CompanyViewModel>, IEnumerable<Company>>(companiesVM);
            await companyRepository.InsertCompaniesAsync(companiesDb);
        }

        public async Task UpdateCompanyAsync(CompanyViewModel companyVM)
        {
            var companieDb = Mapper.Instance.Map<CompanyViewModel, Company>(companyVM);
            await companyRepository.UpdateCompanyAsync(companieDb);
        }

        public async Task UpdateCompaniesAsync(IEnumerable<CompanyViewModel> companiesVM)
        {
            var companiesDb = Mapper.Instance.Map<IEnumerable<CompanyViewModel>, IEnumerable<Company>>(companiesVM);
            await companyRepository.UpdateCompaniesAsync(companiesDb);
        }
    }
}