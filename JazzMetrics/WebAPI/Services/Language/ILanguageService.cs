using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Models.Language;

namespace WebAPI.Services.Language
{
    public interface ILanguageService
    {
        Task<BaseResponseModelGet<LanguageResponseModel>> GetAllLanguages();
    }
}
