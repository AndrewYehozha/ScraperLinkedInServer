using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Services.CompanyService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/v1/companies")]
    public class CompanyV1Controller : ScraperLinkedInApiController
    {
        private readonly ICompanyService companyService;

        public CompanyV1Controller(
            ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        [HttpGet]
        [Route("windows-service-scraper")]
        [Authorize]
        public async Task<IHttpActionResult> GetCompaniesForSearchAsync(int accountId, int companyBatchSize)
        {
            var response = new CompaniesResponse();

            var companiesVM = await companyService.GetCompaniesForSearchAsync(accountId, companyBatchSize);
            response.CompaniesViewModel = companiesVM;
            response.CountCompaniesInProcess = await companyService.CountCompaniesInProcess(accountId);

            return JsonSuccess(response);
        }

        [HttpGet]
        [Route("windows-service-worker")]
        [Authorize]
        public async Task<IHttpActionResult> GetCompaniesForSearchSuitableProfilesAsync(int accountId)
        {
            var response = new CompaniesResponse();

            var companiesVM = await companyService.GetCompaniesForSearchSuitableProfilesAsync(accountId);
            response.CompaniesViewModel = companiesVM;
            response.CountCompaniesInProcess = await companyService.CountCompaniesInProcess(accountId);

            return JsonSuccess(response);
        }

        [HttpGet]
        [Route("count-in-process")]
        [Authorize]
        public async Task<IHttpActionResult> CountCompaniesInProcess(int accountId)
        {
            var response = new CompaniesResponse();

            response.CountCompaniesInProcess = await companyService.CountCompaniesInProcess(accountId);

            return JsonSuccess(response);
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<IHttpActionResult> InsertCompanyAsync(CompanyRequest request)
        {
            var response = new CompanyResponse();

            await companyService.InsertCompanyAsync(request.CompanyViewModel);

            return JsonSuccess(response);
        }

        [HttpPost]
        [Route("import")]
        [Authorize]
        public async Task<IHttpActionResult> InsertCompaniesAsync(CompaniesRequest request)
        {
            var response = new CompaniesResponse();

            await companyService.InsertCompaniesAsync(request.CompaniesViewModel);

            return JsonSuccess(response);
        }

        [HttpPut]
        [Route("")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateCompanyAsync(CompanyRequest request)
        {
            var response = new CompanyResponse();

            await companyService.UpdateCompanyAsync(request.CompanyViewModel);

            return JsonSuccess(response);
        }

        [HttpPut]
        [Route("windows-service-scraper")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateCompaniesAsync(CompaniesRequest request)
        {
            var response = new CompaniesResponse();

            await companyService.UpdateCompaniesAsync(request.CompaniesViewModel);

            return JsonSuccess(response);
        }
    }
}
