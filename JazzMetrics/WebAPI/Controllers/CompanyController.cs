using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Models.Company;
using WebAPI.Services.Companies;
using WebAPI.Services.Error;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : MainController
    {
        private readonly ICompanyService _companyService;

        public CompanyController(IErrorService errorService, ICompanyService companyService) : base(errorService) => _companyService = companyService;

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyModel>> Get(int id, bool lazy = true)
        {
            return await _companyService.Get(id, lazy);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseModelGet<CompanyModel>>> Get(bool lazy = true)
        {
            return await _companyService.GetAll(lazy);
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult<BaseResponseModelPost>> Post(CompanyModel model)
        {
            return await _companyService.Create(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Put(int id, CompanyModel model)
        {
            return await _companyService.Edit(model);
        }

        [HttpDelete("{id}"), AllowAnonymous]
        public async Task<ActionResult<BaseResponseModel>> Delete(int id)
        {
            return await _companyService.Drop(id);
        }
    }
}