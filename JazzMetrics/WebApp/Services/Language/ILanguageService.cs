using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Services.Language
{
    public interface ILanguageService
    {
        Task<List<SelectListItem>> GetLanguagesForSelect();
    }
}
