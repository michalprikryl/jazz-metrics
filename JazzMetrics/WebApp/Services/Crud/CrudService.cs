using Library.Models;
using Library.Networking;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Services.Crud
{
    public class CrudService : ClientApi, ICrudService
    {
        public CrudService(IConfiguration config) : base(config) { }

        public async Task<BaseResponseModelGetAll<T>> GetAll<T>(string jwt, string entity, bool lazy = true)
        {
            BaseResponseModelGetAll<T> result = null;

            await GetToAPI(GetParametersList(GetParameter("lazy", lazy.ToString())), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseResponseModelGetAll<T>>(await httpResult.Content.ReadAsStringAsync());
            }, entity, jwt: jwt);

            return result;
        }

        public async Task<BaseResponseModelGet<T>> Get<T>(int id, string jwt, string entity, bool lazy = true)
        {
            var result = new BaseResponseModelGet<T>();

            await GetToAPI(id, GetParametersList(GetParameter("lazy", lazy.ToString())), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseResponseModelGet<T>>(await httpResult.Content.ReadAsStringAsync());
            }, entity, jwt: jwt);

            return result;
        }

        public async Task<BaseResponseModelPost> Create<T>(T model, string jwt, string entity)
        {
            BaseResponseModelPost result = new BaseResponseModelPost();

            await PostToAPI(SerializeObjectToJSON(model), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseResponseModelPost>(await httpResult.Content.ReadAsStringAsync());
            }, entity, jwt: jwt);

            return result;
        }

        public async Task<BaseResponseModel> Edit<T>(int id, T model, string jwt, string entity)
        {
            BaseResponseModel result = new BaseResponseModel();

            await PutToAPI(id, SerializeObjectToJSON(model), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseResponseModel>(await httpResult.Content.ReadAsStringAsync());
            }, entity, jwt: jwt);

            return result;
        }

        public async Task<BaseResponseModel> Drop(int id, string jwt, string entity)
        {
            BaseResponseModel result = new BaseResponseModel();

            await DeleteToAPI(id, async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseResponseModel>(await httpResult.Content.ReadAsStringAsync());
            }, entity, jwt: jwt);

            return result;
        }

        public async Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> model, string jwt, string entity)
        {
            BaseResponseModel result = new BaseResponseModel();

            await PatchToAPI(id, model, async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseResponseModel>(await httpResult.Content.ReadAsStringAsync());
            }, entity, jwt: jwt);

            return result;
        }
    }
}
