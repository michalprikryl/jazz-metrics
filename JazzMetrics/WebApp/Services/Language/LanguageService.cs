using Library.Networking;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Language;

namespace WebApp.Services.Language
{
    public class LanguageService : ClientApi, ILanguageService
    {
        public LanguageService(IConfiguration config) : base(config, "language") { }

        public async Task<List<LanguageModel>> GetAllLanguages()
        {
            List<LanguageModel> languages = new List<LanguageModel>();

            await GetToAPI(async (httpResult) =>
            {
                languages = JsonConvert.DeserializeObject<BaseApiResultGet<LanguageModel>>(await httpResult.Content.ReadAsStringAsync()).Values;
            });

            return languages;
        }
    }
}
