using Database;
using Database.DAO;
using Library;
using Library.Jazz;
using Library.Models;
using Library.Models.AffectedFields;
using Library.Models.AspiceProcesses;
using Library.Models.Metric;
using Library.Models.MetricColumn;
using Library.Models.MetricType;
using Library.Networking;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Services.AffectedFields;
using WebAPI.Services.AspiceProcesses;
using WebAPI.Services.Helper;
using WebAPI.Services.Helpers;
using WebAPI.Services.MetricTypes;

namespace WebAPI.Services.Metrics
{
    public class MetricService : BaseDatabase, IMetricService, IUser
    {
        private readonly IMetricTypeService _metricTypeService;
        private readonly IAspiceProcessService _aspiceProcessService;
        private readonly IAffectedFieldService _affectedFieldService;

        public CurrentUser CurrentUser { get; set; }

        public MetricService(JazzMetricsContext db, IAspiceProcessService aspiceProcessService, IMetricTypeService metricTypeService, IAffectedFieldService affectedFieldService,
            IHttpContextAccessor contextAccessor, IHelperService helperService) : base(db)
        {
            _metricTypeService = metricTypeService;
            _aspiceProcessService = aspiceProcessService;
            _affectedFieldService = affectedFieldService;
            CurrentUser = helperService.GetCurrentUser(contextAccessor.HttpContext.User.GetId());
        }

        public async Task<BaseResponseModelGetAll<MetricModel>> GetAll(bool lazy)
        {
            var response = new BaseResponseModelGetAll<MetricModel> { Values = new List<MetricModel>() };

            foreach (var item in await Database.Metric.Where(m => m.Public || m.CompanyId == CurrentUser.CompanyId).ToListAsync())
            {
                MetricModel metric = ConvertToModel(item);

                if (!lazy)
                {
                    metric.MetricType = GetMetricType(item.MetricType);
                    metric.AspiceProcess = GetAspiceProcess(item.AspiceProcess);
                    metric.AffectedField = GetAffectedField(item.AffectedField);
                    metric.Columns = GetMetricColumns(item.MetricColumn);
                }

                response.Values.Add(metric);
            }

            return response;
        }

        public async Task<BaseResponseModelGet<MetricModel>> Get(int id, bool lazy)
        {
            var response = new BaseResponseModelGet<MetricModel>();

            Metric metric = await Load(id, response);
            if (metric != null)
            {
                response.Value = ConvertToModel(metric);

                if (!lazy)
                {
                    response.Value.MetricType = GetMetricType(metric.MetricType);
                    response.Value.AspiceProcess = GetAspiceProcess(metric.AspiceProcess);
                    response.Value.AffectedField = GetAffectedField(metric.AffectedField);
                    response.Value.Columns = GetMetricColumns(metric.MetricColumn);
                }
            }

            return response;
        }

        public async Task<BaseResponseModelPost> Create(MetricModel request)
        {
            BaseResponseModelPost response = new BaseResponseModelPost();

            if (request.Validate())
            {
                if (await CheckAspiceProcess(request.AspiceProcessId, response))
                {
                    if (await CheckMetricType(request.MetricTypeId, response))
                    {
                        if (await CheckAffectedField(request.AffectedFieldId, response))
                        {
                            if (await CheckMetricIdentificator(request.Identificator, response))
                            {
                                if (CheckMetricColumns(request.Columns, response))
                                {
                                    Metric metric = new Metric
                                    {
                                        Name = request.Name,
                                        Description = request.Description,
                                        Identificator = request.Identificator,
                                        RequirementGroup = request.RequirementGroup,
                                        AspiceProcessId = request.AspiceProcessId,
                                        AffectedFieldId = request.AffectedFieldId,
                                        MetricTypeId = request.MetricTypeId,
                                        Public = CurrentUser.CompanyId.HasValue ? request.Public : true,
                                        CompanyId = CurrentUser.CompanyId
                                    };

                                    for (int i = 0; i < request.Columns.Count; i++)
                                    {
                                        await CreateMetricColumn(metric, request.Columns[i]);
                                    }

                                    await Database.Metric.AddAsync(metric);

                                    await Database.SaveChangesAsync();

                                    response.Id = metric.Id;
                                    response.Message = "Metric was successfully created!";
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Some of the required properties is not present!";
            }

            return response;
        }

        public async Task<BaseResponseModel> Edit(MetricModel request)
        {
            BaseResponseModel response = new BaseResponseModel();

            if (request.Validate())
            {
                if (await CheckAspiceProcess(request.AspiceProcessId, response))
                {
                    if (await CheckMetricType(request.MetricTypeId, response))
                    {
                        if (await CheckAffectedField(request.AffectedFieldId, response))
                        {
                            Metric metric = await Load(request.Id, response);
                            if (metric != null && CheckMetricForCrud(metric, response))
                            {
                                if (await CheckMetricIdentificator(request.Identificator, response, metric.Id) && CheckMetricColumns(request.Columns, response))
                                {
                                    metric.Identificator = request.Identificator;
                                    metric.Name = request.Name;
                                    metric.Description = request.Description;
                                    metric.RequirementGroup = request.RequirementGroup;
                                    metric.AspiceProcessId = request.AspiceProcessId;
                                    metric.AffectedFieldId = request.AffectedFieldId;
                                    //metric.MetricTypeId = request.MetricTypeId; -- nelze menit z duvodu, ze by potom nesedely hodnoty columns - musi se mazat columns
                                    metric.Public = CurrentUser.CompanyId.HasValue ? request.Public : true;

                                    //smazane
                                    var deletedIds = metric.MetricColumn.Select(c => c.Id).Except(request.Columns.Select(c => c.Id));
                                    Database.MetricColumn.RemoveRange(metric.MetricColumn.Where(c => deletedIds.Contains(c.Id)));

                                    //nove a editovane
                                    for (int i = 0; i < request.Columns.Count; i++)
                                    {
                                        if (request.Columns[i].Id == 0)
                                        {
                                            await CreateMetricColumn(metric, request.Columns[i]);
                                        }
                                        else
                                        {
                                            MetricColumn column = metric.MetricColumn.First(c => c.Id == request.Columns[i].Id);
                                            column.Value = request.Columns[i].Value ?? string.Empty;
                                            column.FieldName = request.Columns[i].FieldName;
                                            column.NumberFieldName = request.Columns[i].NumberFieldName;

                                            if (!string.IsNullOrEmpty(request.Columns[i].CoverageName))
                                            {
                                                column.DivisorValue = request.Columns[i].DivisorValue ?? JazzService.ALL_VALUES;
                                                column.DivisorFieldName = request.Columns[i].DivisorFieldName ?? string.Empty;
                                                column.CoverageName = request.Columns[i].CoverageName;
                                            }
                                        }
                                    }

                                    await Database.SaveChangesAsync();

                                    response.Message = "Metric was successfully edited!";
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Some of the required properties is not present!";
            }

            return response;
        }

        public async Task<BaseResponseModel> Drop(int id)
        {
            BaseResponseModel response = new BaseResponseModel();

            Metric metric = await Load(id, response);
            if (metric != null && CheckMetricForCrud(metric, response))
            {
                if (metric.ProjectMetric.Count == 0)
                {
                    Database.MetricColumn.RemoveRange(metric.MetricColumn);

                    Database.Metric.Remove(metric);

                    await Database.SaveChangesAsync();

                    response.Message = "Metric was successfully deleted!";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Cannot delete metric, because some projects use this metric!";
                }
            }

            return response;
        }

        public Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Metric> Load(int id, BaseResponseModel response)
        {
            Metric metric = await Database.Metric.FirstOrDefaultAsync(m => m.Id == id && (m.CompanyId == CurrentUser.CompanyId || m.Public));
            if (metric == null)
            {
                response.Success = false;
                response.Message = "Unknown metric!";
            }

            return metric;
        }

        private async Task CreateMetricColumn(Metric metric, MetricColumnModel model)
        {
            MetricColumn metricColumn = new MetricColumn
            {
                Metric = metric,
                Value = model.Value ?? string.Empty,
                FieldName = model.FieldName,
                NumberFieldName = model.NumberFieldName
            };

            if (!string.IsNullOrEmpty(model.CoverageName))
            {
                metricColumn.DivisorValue = model.DivisorValue ?? JazzService.ALL_VALUES;
                metricColumn.DivisorFieldName = model.DivisorFieldName ?? string.Empty;
                metricColumn.CoverageName = model.CoverageName;
            }

            await Database.MetricColumn.AddAsync(metricColumn);
        }

        private bool CheckMetricForCrud(Metric metric, BaseResponseModel response)
        {
            if (metric.CompanyId != CurrentUser.CompanyId)
            {
                response.Success = false;
                response.Message = "Metric belongs to different company!";

                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckMetricColumns(List<MetricColumnModel> columns, BaseResponseModel response)
        {
            if (columns != null && columns.Count > 0)
            {
                if (columns.All(c => c.Validate()))
                {
                    return true;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Some of metric columns are note valid!";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Metric must have at least one column!";
            }

            return false;
        }

        private async Task<bool> CheckAspiceProcess(int aspiceProcessId, BaseResponseModel response)
        {
            if (await Database.AspiceProcess.AnyAsync(a => a.Id == aspiceProcessId))
            {
                return true;
            }
            else
            {
                response.Success = false;
                response.Message = "Unknown Automotive SPICE process!";

                return false;
            }
        }

        private async Task<bool> CheckMetricType(int metricTypeId, BaseResponseModel response)
        {
            if (await Database.MetricType.AnyAsync(a => a.Id == metricTypeId))
            {
                return true;
            }
            else
            {
                response.Success = false;
                response.Message = "Unknown metric type!";

                return false;
            }
        }

        private async Task<bool> CheckMetricIdentificator(string identificator, BaseResponseModel response, int id = 0)
        {
            if (await Database.Metric.AnyAsync(a => a.Identificator == identificator && a.Id != id))
            {
                response.Success = false;
                response.Message = "Metric identificator must be unique!";

                return false;
            }
            else
            {
                return true;
            }
        }

        private async Task<bool> CheckAffectedField(int affectedFieldId, BaseResponseModel response)
        {
            if (await Database.AffectedField.AnyAsync(a => a.Id == affectedFieldId))
            {
                return true;
            }
            else
            {
                response.Success = false;
                response.Message = "Unknown affected field of metric!";

                return false;
            }
        }

        private MetricTypeModel GetMetricType(MetricType metricType) => _metricTypeService.ConvertToModel(metricType);

        private AspiceProcessModel GetAspiceProcess(AspiceProcess aspiceProcess) => _aspiceProcessService.ConvertToModel(aspiceProcess);

        private AffectedFieldModel GetAffectedField(AffectedField affectedField) => _affectedFieldService.ConvertToModel(affectedField);

        public List<MetricColumnModel> GetMetricColumns(ICollection<MetricColumn> metricColumns) => metricColumns.Select(c => ConvertMetricColumn(c)).ToList();

        public MetricModel ConvertToModel(Metric dbModel)
        {
            return new MetricModel
            {
                Id = dbModel.Id,
                Name = dbModel.Name,
                Description = dbModel.Description,
                Identificator = dbModel.Identificator,
                RequirementGroup = dbModel.RequirementGroup,
                AspiceProcessId = dbModel.AspiceProcessId,
                AffectedFieldId = dbModel.AffectedFieldId,
                MetricTypeId = dbModel.MetricTypeId,
                CompanyId = dbModel.CompanyId,
                Public = dbModel.Public
            };
        }

        public MetricColumnModel ConvertMetricColumn(MetricColumn metricColumn) =>
            new MetricColumnModel
            {
                Id = metricColumn.Id,
                Value = metricColumn.Value,
                FieldName = metricColumn.FieldName,
                NumberFieldName = metricColumn.NumberFieldName,
                DivisorValue = metricColumn.DivisorValue,
                DivisorFieldName = metricColumn.DivisorFieldName,
                CoverageName = metricColumn.CoverageName,
                MetricId = metricColumn.MetricId
            };
    }
}
