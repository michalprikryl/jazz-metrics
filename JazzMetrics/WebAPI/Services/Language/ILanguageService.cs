using Library.Models;
using Library.Models.Language;
using System.Threading.Tasks;

namespace WebAPI.Services.Language
{
    public interface ILanguageService
    {
        Task<BaseResponseModelGetAll<LanguageModel>> GetAllLanguages();
    }
}
