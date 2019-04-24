using Database;
using Database.DAO;
using Library.Models;
using Library.Models.ProjectMetricSnapshots;
using Library.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;
using WebAPI.Services.Metrics;

namespace WebAPI.Services.ProjectMetricSnapshots
{
    public class ProjectMetricSnapshotService : BaseDatabase, IProjectMetricSnapshotService
    {
        public ProjectMetricSnapshotService(JazzMetricsContext db) : base(db) { }

        public ProjectMetricSnapshotModel ConvertToModel(ProjectMetricSnapshot dbModel)
        {
            ProjectMetricSnapshotModel snapshot = new ProjectMetricSnapshotModel
            {
                Id = dbModel.Id,
                InsertionDate = dbModel.InsertionDate,
                ProjectMetricId = dbModel.ProjectMetricId
            };

            snapshot.Values = dbModel.ProjectMetricColumnValue.Select(v =>
                new ProjectMetricColumnValueModel
                {
                    Id = v.Id,
                    Value = v.Value,
                    MetricColumnId = v.MetricColumnId,
                    ProjectMetricSnapshotId = v.ProjectMetricSnapshotId
                }).ToList();

            return snapshot;
        }

        public Task<BaseResponseModelPost> Create(ProjectMetricSnapshotModel request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModel> Drop(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModel> Edit(ProjectMetricSnapshotModel request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModelGet<ProjectMetricSnapshotModel>> Get(int id, bool lazy)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModelGetAll<ProjectMetricSnapshotModel>> GetAll(bool lazy)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectMetricSnapshot> Load(int id, BaseResponseModel response, bool tracking = true, bool lazy = true)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            throw new NotImplementedException();
        }
    }
}
