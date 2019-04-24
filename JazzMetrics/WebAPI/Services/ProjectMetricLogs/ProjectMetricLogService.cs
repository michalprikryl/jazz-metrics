using Database.DAO;
using Library.Models;
using Library.Models.ProjectMetricLogs;
using Library.Networking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Services.ProjectMetricLogs
{
    public class ProjectMetricLogService : IProjectMetricLogService
    {
        public ProjectMetricLogModel ConvertToModel(ProjectMetricLog dbModel)
        {
            return new ProjectMetricLogModel
            {
                Id = dbModel.Id,
                Message = dbModel.Message,
                CreateDate = dbModel.CreateDate,
                Warning = dbModel.Warning,
                ProjectMetricId = dbModel.ProjectMetricId
            };
        }

        public Task<BaseResponseModelPost> Create(ProjectMetricLogModel request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModel> Drop(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModel> Edit(ProjectMetricLogModel request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModelGet<ProjectMetricLogModel>> Get(int id, bool lazy)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModelGetAll<ProjectMetricLogModel>> GetAll(bool lazy)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectMetricLog> Load(int id, BaseResponseModel response, bool tracking = true, bool lazy = true)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            throw new NotImplementedException();
        }
    }
}
