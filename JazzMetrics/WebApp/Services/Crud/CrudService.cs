using Library.Networking;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Services.Crud
{
    public class CrudService : ClientApi, ICrudService
    {
        public CrudService(IConfiguration config) : base(config) { }

        public async Task<BaseApiResultGet<T>> GetAll<T>(string jwt, string entity, bool lazy = true)
        {
            BaseApiResultGet<T> result = null;

            await GetToAPI(GetParametersList(GetParameter("lazy", lazy.ToString())), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResultGet<T>>(await httpResult.Content.ReadAsStringAsync());
            }, entity, jwt: jwt);

            return result;
        }

        public async Task<T> Get<T>(int id, string jwt, string entity, bool lazy = true)
        {
            T result = default(T);

            await GetToAPI(id, GetParametersList(GetParameter("lazy", lazy.ToString())), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<T>(await httpResult.Content.ReadAsStringAsync());
            }, entity, jwt: jwt);

            return result;
        }

        public async Task<BaseApiResultPost> Create<T>(T model, string jwt, string entity)
        {
            BaseApiResultPost result = new BaseApiResultPost();

            await PostToAPI(SerializeObjectToJSON(model), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResultPost>(await httpResult.Content.ReadAsStringAsync());
            }, entity, jwt: jwt);

            return result;
        }

        public async Task<BaseApiResult> Edit<T>(int id, T model, string jwt, string entity)
        {
            BaseApiResult result = new BaseApiResult();

            await PutToAPI(id, SerializeObjectToJSON(model), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResult>(await httpResult.Content.ReadAsStringAsync());
            }, entity, jwt: jwt);

            return result;
        }

        public async Task<BaseApiResult> Drop(int id, string jwt, string entity)
        {
            BaseApiResult result = new BaseApiResult();

            await DeleteToAPI(id, async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResult>(await httpResult.Content.ReadAsStringAsync());
            }, entity, jwt: jwt);

            return result;
        }

        public async Task<BaseApiResult> PartialEdit(int id, List<PatchModel> model, string jwt, string entity)
        {
            BaseApiResult result = new BaseApiResult();

            await PatchToAPI(id, model, async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResult>(await httpResult.Content.ReadAsStringAsync());
            }, entity, jwt: jwt);

            return result;
        }
    }
}
