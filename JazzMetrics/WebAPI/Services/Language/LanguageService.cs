using Database;
using Library.Models;
using Library.Models.Language;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Language
{
    /// <summary>
    /// servis pro praci s DB tabulkou Language
    /// </summary>
    public class LanguageService : BaseDatabase, ILanguageService
    {
        public LanguageService(JazzMetricsContext db) : base(db) { }

        public async Task<BaseResponseModelGetAll<LanguageModel>> GetAllLanguages()
        {
            return new BaseResponseModelGetAll<LanguageModel>
            {
                Values = await Database.Language.Select(l => new LanguageModel
                {
                    Id = l.Id,
                    Iso6391code = l.Iso6391code,
                    Iso6393code = l.Iso6393code,
                    Name = l.Name
                }).ToListAsync()
            };
        }
    }
}
