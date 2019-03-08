using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Language;

namespace WebApp.Services.Language
{
    public interface ILanguageService
    {
        Task<BaseApiResultGet<LanguageModel>> GetAllLanguages();
        Task<List<SelectListItem>> GetLanguagesForSelect();
    }
}
