using Library.Models;
using Library.Models.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Services.Companies;
using WebAPI.Services.Helper;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : MainController
    {
        private readonly ICompanyService _companyService;

        public CompanyController(IHelperService helperService, ICompanyService companyService) : base(helperService) => _companyService = companyService;

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponseModelGet<CompanyModel>>> Get(int id, bool lazy = true)
        {
            return await _companyService.Get(id, lazy);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseModelGetAll<CompanyModel>>> Get(bool lazy = true)
        {
            return await _companyService.GetAll(lazy);
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult<BaseResponseModelPost>> Post([FromBody]CompanyModel model)
        {
            return await _companyService.Create(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Put(int id, [FromBody]CompanyModel model)
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