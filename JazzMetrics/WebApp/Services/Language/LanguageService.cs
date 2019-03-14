using Library.Models.Language;
using Library.Networking;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Services.Crud;

namespace WebApp.Services.Language
{
    public class LanguageService : ClientApi, ILanguageService
    {
        private readonly ICrudService _crudService;

        private static string LanguageEntity => "language";

        public LanguageService(IConfiguration config, ICrudService crudService) : base(config, LanguageEntity) => _crudService = crudService;

        public async Task<List<SelectListItem>> GetLanguagesForSelect()
        {
            var response = await _crudService.GetAll<LanguageModel>(null, LanguageEntity);
            if (response.Success)
            {
                return response.Values.Select(v =>
                    new SelectListItem
                    {
                        Value = v.Id.ToString(),
                        Text = v.ToString()
                    }).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
