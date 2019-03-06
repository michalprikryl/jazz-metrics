using Library.Networking;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
    }
}
