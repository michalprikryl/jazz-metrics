using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Models.Language;
using WebAPI.Services.Error;
using WebAPI.Services.Language;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageController : MainController
    {
        private readonly ILanguageService _languageService;

        public LanguageController(IErrorService errorService, ILanguageService languageService) : base(errorService)
        {
            _languageService = languageService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult<BaseResponseModelGet<LanguageResponseModel>>> Get()
        {
            return await _languageService.GetAllLanguages();
        }
    }
}