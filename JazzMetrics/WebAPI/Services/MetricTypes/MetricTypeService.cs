using Database;
using Database.DAO;
using Library;
using Library.Models;
using Library.Models.MetricType;
using Library.Networking;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.MetricTypes
{
    public class MetricTypeService : BaseDatabase, IMetricTypeService
    {
        public MetricTypeService(JazzMetricsContext db) : base(db) { }

        public async Task<BaseResponseModelGetAll<MetricTypeModel>> GetAll(bool lazy)
        {
            return new BaseResponseModelGetAll<MetricTypeModel>
            {
                Values = await Database.MetricType.Select(a => ConvertToModel(a)).ToListAsync()
            };
        }

        public async Task<BaseResponseModelGet<MetricTypeModel>> Get(int id, bool lazy)
        {
            var response = new BaseResponseModelGet<MetricTypeModel>();

            MetricType metricType = await Load(id, response, false, lazy);
            if (metricType != null)
            {
                response.Value = ConvertToModel(metricType);
            }

            return response;
        }

        public async Task<BaseResponseModelPost> Create(MetricTypeModel request)
        {
            BaseResponseModelPost response = new BaseResponseModelPost();

            if (request.Validate())
            {
                MetricType metricType = new MetricType
                {
                    Name = request.Name,
                    Description = request.Description
                };

                await Database.MetricType.AddAsync(metricType);

                await Database.SaveChangesAsync();

                response.Id = metricType.Id;
                response.Message = "Metric type was successfully created!";
            }
            else
            {
                response.Success = false;
                response.Message = "Some of the required properties is not present!";
            }

            return response;
        }

        public async Task<BaseResponseModel> Edit(MetricTypeModel request)
        {
            BaseResponseModel response = new BaseResponseModel();

            if (request.Validate())
            {
                MetricType metricType = await Load(request.Id, response);
                if (metricType != null)
                {
                    metricType.Name = request.Name;
                    metricType.Description = request.Description;

                    await Database.SaveChangesAsync();

                    response.Message = "Metric type was successfully edited!";
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

            MetricType metricType = await Load(id, response);
            if (metricType != null)
            {
                if (metricType.Metric.Count == 0)
                {
                    Database.MetricType.Remove(metricType);

                    await Database.SaveChangesAsync();

                    response.Message = "Metric type was successfully deleted!";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Cannot delete metric type, because some metrics use this type!";
                }
            }

            return response;
        }

        public Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<MetricType> Load(int id, BaseResponseModel response, bool tracking = true, bool lazy = true)
        {
            MetricType metricType = await Database.MetricType.FirstOrDefaultAsyncSpecial(m => m.Id == id, tracking);
            if (metricType == null)
            {
                response.Success = false;
                response.Message = "Unknown metric type!";
            }

            return metricType;
        }

        public MetricTypeModel ConvertToModel(MetricType dbModel)
        {
            return new MetricTypeModel
            {
                Id = dbModel.Id,
                Name = dbModel.Name,
                Description = dbModel.Description
            };
        }
    }
}
