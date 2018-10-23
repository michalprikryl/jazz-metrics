using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace WebApp.Classes.Test
{
    /// <summary>
    /// trida pro testovani pripojeni na API a DB  (kdyz nejde API, tak nejde ani DB, ale pokud jde API, tak nemusi jit DB)
    /// </summary>
    public class TestConnection : ClientAPI
    { 
        public TestConnection(string controller = "test", bool useAuthetificationHeader = true, string jwt = null) : base(controller, useAuthetificationHeader, jwt) { }

        /// <summary>
        /// metoda pro testovani pripojeni na API
        /// </summary>
        /// <returns>vysledek testu pripojeni navraceny z API</returns>
        public async Task<TestResult> TestConnectionToAPI()
        {
            TestResult result = new TestResult();

            await GetToAPI(null, (task) =>
            {
                var httpResult = task.Result;
                if (httpResult.StatusCode == HttpStatusCode.OK)
                {
                    TestResultAPI resultAPI = JsonConvert.DeserializeObject<TestResultAPI>(httpResult.Content.ReadAsStringAsync().Result);
                    result.ConnectionDB = resultAPI.ConnectionDB;
                    result.MessageDB = resultAPI.MessageDB ?? "Připojení je v pořádku.";
                    result.ConnectionAPI = true;
                }
                else
                {
                    result.ConnectionDB = false;
                    result.MessageDB = "Nelze získat informace o připojení k DB.";
                    result.ConnectionAPI = false;
                }

                result.HTTPResponseAPI = (int)httpResult.StatusCode;
                result.MessageAPI = Enum.GetName(typeof(HttpStatusCode), httpResult.StatusCode);
            });

            return result;
        }
    }
}