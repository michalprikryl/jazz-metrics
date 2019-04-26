using Library.Models;
using Library.Models.Language;
using System.Threading.Tasks;

namespace WebAPI.Services.Language
{
    /// <summary>
    /// interface pro servis pro praci s DB tabulkou Language
    /// </summary>
    public interface ILanguageService
    {
        /// <summary>
        /// vrati vsechny jazyky z DB
        /// </summary>
        /// <returns></returns>
        Task<BaseResponseModelGetAll<LanguageModel>> GetAllLanguages();
    }
}
