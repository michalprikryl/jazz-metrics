using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Models.Language;

namespace WebApp.Services.Language
{
    public interface ILanguageService
    {
        Task<List<LanguageModel>> GetAllLanguages();
    }
}
