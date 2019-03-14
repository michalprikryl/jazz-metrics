using Library.Models;
using Library.Models.Language;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Services.Helper;
using WebAPI.Services.Language;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageController : MainController
    {
        private readonly ILanguageService _languageService;

        public LanguageController(IHelperService helperService, ILanguageService languageService) : base(helperService)
        {
            _languageService = languageService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult<BaseResponseModelGetAll<LanguageModel>>> Get()
        {
            return await _languageService.GetAllLanguages();
        }
    }
}