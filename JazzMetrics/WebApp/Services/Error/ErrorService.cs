using Library.Networking;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Error;

namespace WebApp.Services.Error
{
    /// <summary>
    /// trida slouzi pro odesilani chyb na API
    /// </summary>
    public class ErrorService : ClientApi, IErrorService
    {
        public ErrorService(IConfiguration config) : base(config, "error") { }

        /// <summary>
        /// vytvori error z modelu ziskaneho z view
        /// </summary>
        /// <param name="model">objekt s informacemi o chybe</param>
        /// <param name="userID">ID uzivatele, pod nimz nastala chyba</param>
        /// <returns>objekt s informaci o zpracovani pozadavku</returns>
        public async Task<BaseApiResult> CreateError(ErrorJsModel model, string userID)
        {
            ErrorModel modelAPI = new ErrorModel
            {
                ExceptionMessage = model.Message,
                InnerExceptionMessage = model.InnerMessage,
                Function = model.Function,
                Module = model.Module,
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
        public async Task<BaseApiResult> CreateError(ErrorModel model)
        {
            BaseApiResult result = new BaseApiResult();

            model.User = $"JazzMetrics - {Configuration["Version"]} -> {model.User}";

            await PostToAPI(SerializeObjectToJSON(model), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResult>(await httpResult.Content.ReadAsStringAsync());
            });

            return result;
        }
    }
}
