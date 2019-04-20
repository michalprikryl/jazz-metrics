using Library.Models;
using Library.Models.AppError;
using Library.Networking;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WebApp.Services.Error
{
    /// <summary>
    /// trida slouzi pro odesilani chyb na API
    /// </summary>
    public class ErrorService : ClientApi, IErrorService
    {
        public const string ErrorEntity = "apperror";

        public ErrorService(IConfiguration config) : base(config, ErrorEntity) { }

        /// <summary>
        /// vytvori error primo z chyby
        /// </summary>
        /// <param name="model">objekt reprezentujici chybu</param>
        /// <returns>objekt s informaci o zpracovani</returns>
        public async Task<BaseResponseModel> CreateError(AppErrorModel model)
        {
            BaseResponseModel result = new BaseResponseModel();

            model.User = $"JazzMetrics - {Configuration["Version"]} -> {model.User}";

            await PostToAPI(SerializeObjectToJSON(model), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseResponseModel>(await httpResult.Content.ReadAsStringAsync());
            });

            return result;
        }
    }
}
