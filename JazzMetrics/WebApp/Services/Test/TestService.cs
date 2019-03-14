using Library.Models.Test;
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
    public class TestService : ClientApi, ITestService
    {
        public static string TestEntity => "test";

        public TestService(IConfiguration config) : base(config, TestEntity) { }

        /// <summary>
        /// metoda pro testovani pripojeni na API
        /// </summary>
        /// <returns>vysledek testu pripojeni navraceny z API</returns>
        public async Task<TestViewModel> TestConnectionToAPI()
        {
            TestViewModel result = new TestViewModel { ApiResultMessage = new ResultViewModel(), DbResultMessage = new ResultViewModel() };

            await GetToAPI(async (httpResult) =>
            {
                if (httpResult.StatusCode == HttpStatusCode.OK)
                {
                    TestModel resultAPI = JsonConvert.DeserializeObject<TestModel>(await httpResult.Content.ReadAsStringAsync());
                    result.ConnectionDB = resultAPI.ConnectionDB;
                    result.MessageDB = resultAPI.MessageDB ?? "Connection attempt was successful.";
                    result.ConnectionApi = true;
                }
                else
                {
                    result.ConnectionDB = false;
                    result.MessageDB = "Cannot retrieve information about connection to Database.";
                    result.ConnectionApi = false;
                }

                result.HTTPResponseApi = (int)httpResult.StatusCode;
                result.MessageApi = Enum.GetName(typeof(HttpStatusCode), httpResult.StatusCode);

                result.DbResultMessage = result.ConnectionDB ? GetWorkingModel() : GetNotWorkingModel();
                result.ApiResultMessage = result.ConnectionApi ? GetWorkingModel() : GetNotWorkingModel();
            });

            return result;
        }

        private ResultViewModel GetWorkingModel() =>
            new ResultViewModel
            {
                Color = "chartreuse",
                CssClass = "fa-check",
                Message = "Connection is working"
            };

        private ResultViewModel GetNotWorkingModel() =>
            new ResultViewModel
            {
                Color = "tomato",
                CssClass = "fa-times",
                Message = "Connection is not working"
            };
    }
}
