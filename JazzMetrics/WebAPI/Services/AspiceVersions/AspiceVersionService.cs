using Database;
using Database.DAO;
using Library.Models;
using Library.Models.AspiceVersions;
using Library.Networking;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.AspiceVersions
{
    public class AspiceVersionService : BaseDatabase, IAspiceVersionService
    {
        public AspiceVersionService(JazzMetricsContext db) : base(db) { }

        public async Task<BaseResponseModelGetAll<AspiceVersionModel>> GetAll(bool lazy)
        {
            return new BaseResponseModelGetAll<AspiceVersionModel>
            {
                Values = (await Database.AspiceVersion.ToListAsync()).Select(a => ConvertToModel(a)).ToList()
            };
        }

        public async Task<BaseResponseModelGet<AspiceVersionModel>> Get(int id, bool lazy)
        {
            var response = new BaseResponseModelGet<AspiceVersionModel>();

            AspiceVersion aspiceVersion = await Load(id, response);
            if (aspiceVersion != null)
            {
                response.Value = ConvertToModel(aspiceVersion);
            }

            return response;
        }

        public async Task<BaseResponseModelPost> Create(AspiceVersionModel request)
        {
            BaseResponseModelPost response = new BaseResponseModelPost();

            if (request.Validate())
            {
                AspiceVersion aspiceVersion = new AspiceVersion
                {
                    ReleaseDate = request.ReleaseDate,
                    Description = request.Description,
                    VersionNumber = request.VersionNumber
                };

                await Database.AspiceVersion.AddAsync(aspiceVersion);

                await Database.SaveChangesAsync();

                response.Id = aspiceVersion.Id;
                response.Message = "Automotive SPICE version was successfully created!";
            }
            else
            {
                response.Success = false;
                response.Message = "Some of the required properties is not present!";
            }

            return response;
        }

        public async Task<BaseResponseModel> Edit(AspiceVersionModel request)
        {
            BaseResponseModel response = new BaseResponseModel();

            if (request.Validate())
            {
                AspiceVersion aspiceVersion = await Load(request.Id, response);
                if (aspiceVersion != null)
                {
                    aspiceVersion.ReleaseDate = request.ReleaseDate;
                    aspiceVersion.Description = request.Description;
                    aspiceVersion.VersionNumber = request.VersionNumber;

                    await Database.SaveChangesAsync();

                    response.Message = "Automotive SPICE version was successfully edited!";
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

            AspiceVersion aspiceVersion = await Load(id, response);
            if (aspiceVersion != null)
            {
                if (aspiceVersion.AspiceProcess.Count == 0)
                {
                    Database.AspiceVersion.Remove(aspiceVersion);

                    await Database.SaveChangesAsync();

                    response.Message = "Automotive SPICE version was successfully deleted!";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Cannot delete Automotive SPICE version, because some Automotive SPICE process use this version!";
                }
            }

            return response;
        }

        public Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AspiceVersion> Load(int id, BaseResponseModel response)
        {
            AspiceVersion aspiceVersion = await Database.AspiceVersion.FirstOrDefaultAsync(a => a.Id == id);
            if (aspiceVersion == null)
            {
                response.Success = false;
                response.Message = "Unknown Automotive SPICE version!";
            }

            return aspiceVersion;
        }

        public AspiceVersionModel ConvertToModel(AspiceVersion dbModel)
        {
            return new AspiceVersionModel
            {
                Id = dbModel.Id,
                ReleaseDate = dbModel.ReleaseDate,
                Description = dbModel.Description,
                VersionNumber = dbModel.VersionNumber
            };
        }
    }
}
