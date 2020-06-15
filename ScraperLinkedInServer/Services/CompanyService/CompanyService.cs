using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.ObjectMappers;
using ScraperLinkedInServer.Repositories.CompanyRepository.Interfaces;
using ScraperLinkedInServer.Services.CompanyService.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScraperLinkedInServer.Services.CompanyService
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(
            ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CompanyViewModel> GetCompanyByIdAsync(int id, int accountId)
        {
            var companyDb = await _companyRepository.GetCompanyByIdAsync(id, accountId);
            return Mapper.Instance.Map<Company, CompanyViewModel>(companyDb);
        }

        public async Task<SearchCompaniesResponse> SearchCompaniesAsync(int accountId, SearchCompaniesRequest request)
        {
            return await _companyRepository.SearchCompaniesAsync(accountId, request);
        }

        public async Task<ExportCompaniesResponse> SearchExportCompaniesAsync(int accountId, SearchCompaniesRequest request)
        {
            return await _companyRepository.ExportCompaniesAsync(accountId, request);
        }

        public async Task<IEnumerable<CompanyViewModel>> GetCompaniesForSearchAsync(int accountId, int companyBatchSize)
        {
            var companiesDb = await _companyRepository.GetCompaniesForSearchAsync(accountId, companyBatchSize);
            return Mapper.Instance.Map<IEnumerable<Company>, IEnumerable<CompanyViewModel>>(companiesDb);
        }

        public async Task<IEnumerable<CompanyProfilesViewModel>> GetCompaniesProfilesForSearchAsync(int accountId, int companyBatchSize)
        {
            var companiesDb = await _companyRepository.GetCompaniesProfilesForSearchAsync(accountId, companyBatchSize);
            return Mapper.Instance.Map<IEnumerable<Company>, IEnumerable<CompanyProfilesViewModel>>(companiesDb);
        }

        public async Task<int> GetCountCompaniesInProcess(int accountId)
        {
            return await _companyRepository.GetCountCompaniesInProcess(accountId);
        }

        public async Task InsertCompanyAsync(CompanyViewModel companyVM)
        {
            var companieDb = Mapper.Instance.Map<CompanyViewModel, Company>(companyVM);
            await _companyRepository.InsertCompanyAsync(companieDb);
        }

        public async Task<int> InsertCompaniesAsync(int accountId, IEnumerable<CompanyViewModel> companiesVM)
        {
            var companiesDb = Mapper.Instance.Map<IEnumerable<CompanyViewModel>, IEnumerable<Company>>(companiesVM.Where(x => (x.LinkedInURL != null && x.LinkedInURL.Trim() != "")));
            return await _companyRepository.InsertCompaniesAsync(accountId, companiesDb);
        }

        public async Task UpdateCompanyAsync(CompanyViewModel companyVM)
        {
            var companieDb = Mapper.Instance.Map<CompanyViewModel, Company>(companyVM);
            await _companyRepository.UpdateCompanyAsync(companieDb);
        }

        public async Task UpdateCompaniesAsync(IEnumerable<CompanyViewModel> companiesVM)
        {
            var companiesDb = Mapper.Instance.Map<IEnumerable<CompanyViewModel>, IEnumerable<Company>>(companiesVM);
            await _companyRepository.UpdateCompaniesAsync(companiesDb);
        }

        public async Task UpdateLastPageCompanyAsync(int accountId, int companyId, int lastScrapedPage)
        {
            await _companyRepository.UpdateLastPageCompanyAsync(accountId, companyId, lastScrapedPage);
        }
    }
}