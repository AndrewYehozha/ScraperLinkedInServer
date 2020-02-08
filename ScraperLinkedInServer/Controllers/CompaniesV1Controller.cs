using ScraperLinkedInServer.Extensions;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Response;
using ScraperLinkedInServer.Models.Types;
using ScraperLinkedInServer.Services.CompanyService.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/v1/companies")]
    public class CompaniesV1Controller : ScraperLinkedInApiController
    {
        private readonly ICompanyService companyService;

        public CompaniesV1Controller(
            ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        [HttpGet]
        [Route("windows-service-scraper")]
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> GetCompaniesForSearchAsync(int companyBatchSize)
        {
            var response = new CompaniesResponse();

            var accountId = Identity.ToAccountID();
            var companiesVM = await companyService.GetCompaniesForSearchAsync(accountId, companyBatchSize);
            var countCompaniesInProcess = await companyService.GetCountCompaniesInProcess(accountId);
            response.CompaniesViewModel = companiesVM;
            response.CountCompaniesInProcess = countCompaniesInProcess;

            return JsonSuccess(response);
        }

        [HttpGet]
        [Route("windows-service-worker")]
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> GetCompaniesForSearchSuitableProfilesAsync(int companyBatchSize)
        {
            var response = new CompaniesResponse();

            var accountId = Identity.ToAccountID();
            var companiesVM = await companyService.GetCompaniesForSearchSuitableProfilesAsync(accountId);
            response.CompaniesViewModel = companiesVM;

            return JsonSuccess(response);
        }

        [HttpGet]
        [Route("count-in-process")]
        [Authorize]
        public async Task<IHttpActionResult> GetCountCompaniesInProcess()
        {
            var response = new CompaniesResponse();

            var accountId = Identity.ToAccountID();
            var countCompaniesInProcess = await companyService.GetCountCompaniesInProcess(accountId);
            response.CountCompaniesInProcess = countCompaniesInProcess;

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
        [Authorize(Roles = Roles.WindowsService)]
        public async Task<IHttpActionResult> UpdateCompaniesAsync(CompaniesRequest request)
        {
            var response = new CompaniesResponse();

            await companyService.UpdateCompaniesAsync(request.CompaniesViewModel);

            return JsonSuccess(response);
        }
    }
}
