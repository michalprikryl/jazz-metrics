using Library.Networking;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Setting.AffectedField;
using WebApp.Models.Setting.AspiceVersion;
using WebApp.Models.Setting.MetricType;

namespace WebApp.Services.Setting
{
    public class SettingService : ClientApi, ISettingService
    {
        public SettingService(IConfiguration config) : base(config, null) { }

        #region Affected fields
        public async Task<BaseApiResultGet<AffectedFieldModel>> GetAllAffectedFields(string jwt)
        {
            BaseApiResultGet<AffectedFieldModel> result = new BaseApiResultGet<AffectedFieldModel>();

            await GetToAPI(async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResultGet<AffectedFieldModel>>(await httpResult.Content.ReadAsStringAsync());
            }, "affectedfield", jwt: jwt);

            return result;
        }

        public async Task<AffectedFieldModel> GetAffectedField(int id, string jwt)
        {
            AffectedFieldModel result = new AffectedFieldModel();

            await GetToAPI(id, async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<AffectedFieldModel>(await httpResult.Content.ReadAsStringAsync());
            }, "affectedfield", jwt: jwt);

            return result;
        }

        public async Task<BaseApiResult> CreateAffectedField(AffectedFieldWorkModel model, string jwt)
        {
            BaseApiResult result = new BaseApiResult();

            await PostToAPI(SerializeObjectToJSON(model), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResult>(await httpResult.Content.ReadAsStringAsync());
            }, "affectedfield", jwt: jwt);

            return result;
        }

        public async Task<BaseApiResult> EditAffectedField(AffectedFieldWorkModel model, string jwt)
        {
            BaseApiResult result = new BaseApiResult();

            await PutToAPI(model.Id, SerializeObjectToJSON(model), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResult>(await httpResult.Content.ReadAsStringAsync());
            }, "affectedfield", jwt: jwt);

            return result;
        }

        public async Task<BaseApiResult> DropAffectedField(int id, string jwt)
        {
            BaseApiResult result = new BaseApiResult();

            await DeleteToAPI(id, async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResult>(await httpResult.Content.ReadAsStringAsync());
            }, "affectedfield", jwt: jwt);

            return result;
        }
        #endregion

        #region Metric types
        public async Task<BaseApiResultGet<MetricTypeModel>> GetAllMetricTypes(string jwt)
        {
            BaseApiResultGet<MetricTypeModel> result = new BaseApiResultGet<MetricTypeModel>();

            await GetToAPI(async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResultGet<MetricTypeModel>>(await httpResult.Content.ReadAsStringAsync());
            }, "metrictype", jwt: jwt);

            return result;
        }

        public async Task<MetricTypeModel> GetMetricType(int id, string jwt)
        {
            MetricTypeModel result = new MetricTypeModel();

            await GetToAPI(id, async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<MetricTypeModel>(await httpResult.Content.ReadAsStringAsync());
            }, "metrictype", jwt: jwt);

            return result;
        }

        public async Task<BaseApiResult> CreateMetricType(MetricTypeWorkModel model, string jwt)
        {
            BaseApiResult result = new BaseApiResult();

            await PostToAPI(SerializeObjectToJSON(model), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResult>(await httpResult.Content.ReadAsStringAsync());
            }, "metrictype", jwt: jwt);

            return result;
        }

        public async Task<BaseApiResult> EditMetricType(MetricTypeWorkModel model, string jwt)
        {
            BaseApiResult result = new BaseApiResult();

            await PutToAPI(model.Id, SerializeObjectToJSON(model), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResult>(await httpResult.Content.ReadAsStringAsync());
            }, "metrictype", jwt: jwt);

            return result;
        }

        public async Task<BaseApiResult> DropMetricType(int id, string jwt)
        {
            BaseApiResult result = new BaseApiResult();

            await DeleteToAPI(id, async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResult>(await httpResult.Content.ReadAsStringAsync());
            }, "metrictype", jwt: jwt);

            return result;
        }
        #endregion

        #region Automovice SPICE version
        public async Task<BaseApiResultGet<AspiceVersionModel>> GetAllAspiceVersions(string jwt)
        {
            BaseApiResultGet<AspiceVersionModel> result = new BaseApiResultGet<AspiceVersionModel>();

            await GetToAPI(async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResultGet<AspiceVersionModel>>(await httpResult.Content.ReadAsStringAsync());
            }, "aspiceversion", jwt: jwt);

            return result;
        }

        public async Task<AspiceVersionModel> GetAspiceVersion(int id, string jwt)
        {
            AspiceVersionModel result = new AspiceVersionModel();

            await GetToAPI(id, async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<AspiceVersionModel>(await httpResult.Content.ReadAsStringAsync());
            }, "aspiceversion", jwt: jwt);

            return result;
        }

        public async Task<BaseApiResult> CreateAspiceVersion(AspiceVersionWorkModel model, string jwt)
        {
            BaseApiResult result = new BaseApiResult();

            await PostToAPI(SerializeObjectToJSON(model), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResult>(await httpResult.Content.ReadAsStringAsync());
            }, "aspiceversion", jwt: jwt);

            return result;
        }

        public async Task<BaseApiResult> EditAspiceVersion(AspiceVersionWorkModel model, string jwt)
        {
            BaseApiResult result = new BaseApiResult();

            await PutToAPI(model.Id, SerializeObjectToJSON(model), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResult>(await httpResult.Content.ReadAsStringAsync());
            }, "aspiceversion", jwt: jwt);

            return result;
        }

        public async Task<BaseApiResult> DropAspiceVersion(int id, string jwt)
        {
            BaseApiResult result = new BaseApiResult();

            await DeleteToAPI(id, async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResult>(await httpResult.Content.ReadAsStringAsync());
            }, "aspiceversion", jwt: jwt);

            return result;
        }
        #endregion

        public async Task<List<SelectListItem>> GetAspiceVersions(string jwt)
        {
            var response = await GetAll<BaseApiResultGet<AspiceVersionModel>>(jwt, "aspiceversion");
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

        #region Automovice SPICE processes
        public async Task<T> GetAll<T>(string jwt, string entity)
        {
            T result = default(T);

            await GetToAPI(async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<T>(await httpResult.Content.ReadAsStringAsync());
            }, entity, jwt: jwt);

            return result;
        }

        public async Task<T> Get<T>(int id, string jwt, string entity)
        {
            T result = default(T);

            await GetToAPI(id, async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<T>(await httpResult.Content.ReadAsStringAsync());
            }, entity, jwt: jwt);

            return result;
        }

        public async Task<BaseApiResult> Create<T>(T model, string jwt, string entity)
        {
            BaseApiResult result = new BaseApiResult();

            await PostToAPI(SerializeObjectToJSON(model), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResult>(await httpResult.Content.ReadAsStringAsync());
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
        #endregion
    }
}
