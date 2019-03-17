using Library.Models;
using Library.Models.Metric;
using Library.Models.ProjectUsers;
using Library.Networking;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Services.Crud;
using WebApp.Services.Setting;

namespace WebApp.Services.Project
{
    public class ProjectService : ClientApi, IProjectService
    {
        public const string ProjectEntity = "project";
        public const string ProjectUserEntity = "projectuser";
        public const string ProjectMetricEntity = "projectmetric";

        private readonly ICrudService _crudService;

        public ProjectService(IConfiguration config, ICrudService crudService) : base(config) => _crudService = crudService;

        public async Task<BaseResponseModelGet<ProjectUserModel>> GetProjectUser(int userId, int projectId, string jwt)
        {
            var result = new BaseResponseModelGet<ProjectUserModel>();

            await GetToAPI(GetParametersList(GetParameter("userId", userId.ToString()), GetParameter("projectId", projectId.ToString())), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseResponseModelGet<ProjectUserModel>>(await httpResult.Content.ReadAsStringAsync());
            }, ProjectUserEntity, jwt: jwt);

            return result;
        }

        public async Task<List<SelectListItem>> GetMetricsForSelect(string jwt)
        {
            var response = await _crudService.GetAll<MetricModel>(jwt, SettingService.MetricEntity);
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
    }
}
