﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.DAO;
using Library.Networking;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Models.AspiceVersions;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.AspiceVersions
{
    public class AspiceVersionService : BaseDatabase, IAspiceVersionService
    {
        public AspiceVersionService(JazzMetricsContext db) : base(db) { }

        public async Task<BaseResponseModelGet<AspiceVersionModel>> GetAll(bool lazy)
        {
            return new BaseResponseModelGet<AspiceVersionModel>
            {
                Values = (await Database.AspiceVersion.ToListAsync()).Select(a => ConvertToModel(a)).ToList()
            };
        }

        public async Task<AspiceVersionModel> Get(int id, bool lazy)
        {
            AspiceVersionModel response = new AspiceVersionModel();

            AspiceVersion aspiceVersion = await Load(id, response);
            if (aspiceVersion != null)
            {
                response.Id = aspiceVersion.Id;
                response.ReleaseDate = aspiceVersion.ReleaseDate;
                response.Description = aspiceVersion.Description;
                response.VersionNumber = aspiceVersion.VersionNumber;
            }

            return response;
        }

        public async Task<BaseResponseModelPost> Create(AspiceVersionModel request)
        {
            BaseResponseModelPost response = new BaseResponseModelPost();

            if (request.Validate)
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

            if (request.Validate)
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
                Database.AspiceVersion.Remove(aspiceVersion);

                await Database.SaveChangesAsync();

                response.Message = "Automotive SPICE version was successfully deleted!";
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
