using Database;
using Database.DAO;
using Library;
using Library.Models;
using Library.Models.AffectedFields;
using Library.Networking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.AffectedFields
{
    /// <summary>
    /// servis pro praci s DB tabulkou AffectedField
    /// </summary>
    public class AffectedFieldService : BaseDatabase, IAffectedFieldService
    {
        public AffectedFieldService(JazzMetricsContext db) : base(db) { }

        public async Task<BaseResponseModelGetAll<AffectedFieldModel>> GetAll(bool lazy)
        {
            return new BaseResponseModelGetAll<AffectedFieldModel>
            {
                Values = await Database.AffectedField.Select(a => ConvertToModel(a)).ToListAsyncSpecial()
            };
        }

        public async Task<BaseResponseModelGet<AffectedFieldModel>> Get(int id, bool lazy)
        {
            var response = new BaseResponseModelGet<AffectedFieldModel>();

            AffectedField affectedField = await Load(id, response, false);
            if (affectedField != null)
            {
                response.Value = ConvertToModel(affectedField);
            }

            return response;
        }

        public async Task<BaseResponseModelPost> Create(AffectedFieldModel request)
        {
            BaseResponseModelPost response = new BaseResponseModelPost();

            if (request.Validate())
            {
                AffectedField affectedField = new AffectedField
                {
                    Name = request.Name,
                    Description = request.Description
                };

                await Database.AffectedField.AddAsync(affectedField);

                await Database.SaveChangesAsync();

                response.Id = affectedField.Id;
                response.Message = "Affected field was successfully created!";
            }
            else
            {
                response.Success = false;
                response.Message = "Some of the required properties is not present!";
            }

            return response;
        }

        public async Task<BaseResponseModel> Edit(AffectedFieldModel request)
        {
            BaseResponseModel response = new BaseResponseModel();

            if (request.Validate())
            {
                AffectedField affectedField = await Load(request.Id, response);
                if (affectedField != null)
                {
                    affectedField.Name = request.Name;
                    affectedField.Description = request.Description;

                    await Database.SaveChangesAsync();

                    response.Message = "Affected field was successfully edited!";
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

            AffectedField affectedField = await Load(id, response);
            if (affectedField != null)
            {
                if (affectedField.Metric.Count == 0)
                {
                    Database.AffectedField.Remove(affectedField);

                    await Database.SaveChangesAsync();

                    response.Message = "Affected field was successfully deleted!";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Cannot delete affected field, because some metrics use this field!";
                }
            }

            return response;
        }

        public Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AffectedField> Load(int id, BaseResponseModel response, bool tracking = true, bool lazy = true)
        {
            AffectedField affectedField = await Database.AffectedField.FirstOrDefaultAsyncSpecial(a => a.Id == id, tracking);
            if (affectedField == null)
            {
                response.Success = false;
                response.Message = "Unknown affected field!";
            }

            return affectedField;
        }

        public AffectedFieldModel ConvertToModel(AffectedField dbModel)
        {
            return new AffectedFieldModel
            {
                Id = dbModel.Id,
                Name = dbModel.Name,
                Description = dbModel.Description
            };
        }
    }
}
