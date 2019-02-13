using WebApp.Models.Error;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using JazzMetricsLibrary;

namespace WebApp.Classes.Error
{
    /// <summary>
    /// trida slouzi pro odesilani chyb na API
    /// </summary>
    public class ErrorSender : ClientAPI
    {
        /// <summary>
        /// API controller je zadan, i pouziti HTTP Basic autentifikace
        /// </summary>
        /// <param name="controller">controller, na ktery se zasila pozadavek</param>
        /// <param name="useAuthetificationHeader">pouziti autentifikacni hlavicky</param>
        public ErrorSender(string controller = "error", bool useAuthetificationHeader = true, string jwt = null) : base(controller, useAuthetificationHeader, jwt) { }

        /// <summary>
        /// vytvori error z modelu ziskaneho z view
        /// </summary>
        /// <param name="model">objekt s informacemi o chybe</param>
        /// <param name="userID">ID uzivatele, pod nimz nastala chyba</param>
        /// <returns>objekt s informaci o zpracovani pozadavku</returns>
        public async Task<BaseAPIResult> CreateError(ErrorModel model, string userID)
        {
            ErrorAPI modelAPI = new ErrorAPI
            {
                ExceptionMessage = model.Message,
                InnerExceptionMessage = model.InnerMessage,
                Function = model.Function,
                Module = $"JazzWeb-{model.Module}",
                Time = DateTime.Now,
                User = userID,
                ExceptionType = model.ExceptionType,
                Message = string.Empty
            };

            return await CreateError(modelAPI);
        }

        /// <summary>
        /// vytvori error primo z chyby
        /// </summary>
        /// <param name="model">objekt reprezentujici chybu</param>
        /// <returns>objekt s informaci o zpracovani</returns>
        public async Task<BaseAPIResult> CreateError(ErrorAPI model)
        {
            BaseAPIResult result = new BaseAPIResult();

            await PostToAPI(SerializeObjectToJSON(model), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseAPIResult>(await httpResult.Content.ReadAsStringAsync());
            });

            return result;
        }
    }
}