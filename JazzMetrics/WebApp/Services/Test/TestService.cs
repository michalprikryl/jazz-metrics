using Library.Networking;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using WebApp.Models.Partials;
using WebApp.Models.Test;

namespace WebApp.Services.Test
{
    /// <summary>
    /// trida pro testovani pripojeni na API a DB  (kdyz nejde API, tak nejde ani DB, ale pokud jde API, tak nemusi jit DB)
    /// </summary>
    public class TestService : ClientApiWithConfig, ITestService
    {
        public TestService(IConfiguration config, string controller = "test", string jwt = null) : base(config, controller, jwt) { }

        /// <summary>
        /// metoda pro testovani pripojeni na API
        /// </summary>
        /// <returns>vysledek testu pripojeni navraceny z API</returns>
        public async Task<TestViewModel> TestConnectionToAPI()
        {
            TestViewModel result = new TestViewModel { ApiResultMessage = new ResultViewModel(), DbResultMessage = new ResultViewModel() };

            await GetToAPI(null, async (httpResult) =>
            {
                if (httpResult.StatusCode == HttpStatusCode.OK)
                {
                    TestModel resultAPI = JsonConvert.DeserializeObject<TestModel>(await httpResult.Content.ReadAsStringAsync());
                    result.ConnectionDB = resultAPI.ConnectionDB;
                    result.MessageDB = resultAPI.MessageDB ?? "Připojení je v pořádku.";
                    result.ConnectionApi = true;
                }
                else
                {
                    result.ConnectionDB = false;
                    result.MessageDB = "Nelze získat informace o připojení k DB.";
                    result.ConnectionApi = false;
                }

                result.HTTPResponseApi = (int)httpResult.StatusCode;
                result.MessageApi = Enum.GetName(typeof(HttpStatusCode), httpResult.StatusCode);

                result.DbResultMessage.Message = result.ConnectionDB ? "Připojení je funkční" : "Připojení je nedostupné";
                result.ApiResultMessage.Message = result.ConnectionApi ? "Připojení je funkční" : "Připojení je nedostupné";
            });

            return result;
        }
    }
}
