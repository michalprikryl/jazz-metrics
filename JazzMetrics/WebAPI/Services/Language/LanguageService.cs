using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Models.Language;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Language
{
    public class LanguageService : BaseDatabase, ILanguageService
    {
        public LanguageService(JazzMetricsContext db) : base(db) { }

        public async Task<BaseResponseModelGet<LanguageResponseModel>> GetAllLanguages()
        {
            return new BaseResponseModelGet<LanguageResponseModel>
            {
                Values = await Database.Language.Select(l => new LanguageResponseModel
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
