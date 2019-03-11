using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.DAO;
using Library.Networking;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Models.AffectedFields;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.AffectedFields
{
    public class AffectedFieldService : BaseDatabase, IAffectedFieldService
    {
        public AffectedFieldService(JazzMetricsContext db) : base(db) { }

        public async Task<BaseResponseModelGet<AffectedFieldModel>> GetAll(bool lazy)
        {
            return new BaseResponseModelGet<AffectedFieldModel>
            {
                Values = (await Database.AffectedField.ToListAsync()).Select(a => ConvertToModel(a)).ToList()
            };
        }

        public async Task<AffectedFieldModel> Get(int id, bool lazy)
        {
            AffectedFieldModel response = new AffectedFieldModel();

            AffectedField affectedField = await Load(id, response);
            if (affectedField != null)
            {
                response.Id = affectedField.Id;
                response.Name = affectedField.Name;
                response.Description = affectedField.Description;
            }

            return response;
        }

        public async Task<BaseResponseModelPost> Create(AffectedFieldModel request)
        {
            BaseResponseModelPost response = new BaseResponseModelPost();

            if (request.Validate)
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

            if (request.Validate)
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
                Database.AffectedField.Remove(affectedField);

                await Database.SaveChangesAsync();

                response.Message = "Affected field was successfully deleted!";
            }

            return response;
        }

        public Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AffectedField> Load(int id, BaseResponseModel response)
        {
            AffectedField affectedField = await Database.AffectedField.FirstOrDefaultAsync(a => a.Id == id);
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
