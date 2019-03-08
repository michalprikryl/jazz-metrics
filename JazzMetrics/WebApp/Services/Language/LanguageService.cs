using Library.Networking;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Language;

namespace WebApp.Services.Language
{
    public class LanguageService : ClientApi, ILanguageService
    {
        public LanguageService(IConfiguration config) : base(config, "language") { }

        public async Task<BaseApiResultGet<LanguageModel>> GetAllLanguages()
        {
            BaseApiResultGet<LanguageModel> languages = new BaseApiResultGet<LanguageModel>();

            await GetToAPI(async (httpResult) =>
            {
                languages = JsonConvert.DeserializeObject<BaseApiResultGet<LanguageModel>>(await httpResult.Content.ReadAsStringAsync());
            });

            return languages;
        }

        public async Task<List<SelectListItem>> GetLanguagesForSelect()
        {
            var response = await GetAllLanguages();
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
