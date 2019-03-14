using Library.Models;
using Library.Models.ProjectUsers;
using Library.Networking;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WebApp.Services.Project
{
    public class ProjectService : ClientApi, IProjectService
    {
        public const string ProjectEntity = "project";
        public const string ProjectUserEntity = "projectuser";
        public const string ProjectMetricEntity = "projectmetric";

        public ProjectService(IConfiguration config) : base(config) { }

        public async Task<BaseResponseModelGet<ProjectUserModel>> GetProjectUser(int userId, int projectId, string jwt)
        {
            var result = new BaseResponseModelGet<ProjectUserModel>();

            await GetToAPI(GetParametersList(GetParameter("userId", userId.ToString()), GetParameter("projectId", projectId.ToString())), async (httpResult) =>
            {
                result = JsonConvert.DeserializeObject<BaseResponseModelGet<ProjectUserModel>>(await httpResult.Content.ReadAsStringAsync());
            }, ProjectUserEntity, jwt: jwt);

            return result;
        }
    }
}
