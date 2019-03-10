﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Database;
using Database.DAO;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Models.AspiceProcesses;
using WebAPI.Models.AspiceVersions;
using WebAPI.Services.AspiceVersions;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.AspiceProcesses
{
    public class AspiceProcessService : BaseDatabase, IAspiceProcessService
    {
        private readonly IAspiceVersionService _aspiceVersionService;

        public AspiceProcessService(JazzMetricsContext db, IAspiceVersionService aspiceVersionService) : base(db) => _aspiceVersionService = aspiceVersionService;

        public async Task<BaseResponseModelGet<AspiceProcessModel>> GetAll()
        {
            var response = new BaseResponseModelGet<AspiceProcessModel> { Values = new List<AspiceProcessModel>() };

            foreach (var item in await Database.AspiceProcess.ToListAsync())
            {
                response.Values.Add(
                    new AspiceProcessModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Shortcut = item.Shortcut,
                        Description = item.Description,
                        AspiceVersion = await GetAspiceVersion(item.AspiceVersionId)
                    });
            }

            return response;
        }

        public async Task<AspiceProcessModel> Get(int id)
        {
            AspiceProcessModel response = new AspiceProcessModel();

            AspiceProcess aspiceProcess = await Load(id, response);
            if (aspiceProcess != null)
            {
                response.Id = aspiceProcess.Id;
                response.Name = aspiceProcess.Name;
                response.Shortcut = aspiceProcess.Shortcut;
                response.Description = aspiceProcess.Description;
                response.AspiceVersion = await GetAspiceVersion(aspiceProcess.AspiceVersionId);
            }

            return response;
        }

        public async Task<BaseResponseModel> Create(AspiceProcessModel request)
        {
            BaseResponseModel response = new BaseResponseModel();

            if (request.Validate)
            {
                if (await CheckAspiceVersion(request.AspiceVersionId, response))
                {
                    await Database.AspiceProcess.AddAsync(
                        new AspiceProcess
                        {
                            Name = request.Name,
                            Shortcut = request.Shortcut,
                            Description = request.Description,
                            AspiceVersionId = request.AspiceVersionId
                        });

                    await Database.SaveChangesAsync();

                    response.Message = "Automotive SPICE process was successfully created!";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Some of the required properties is not present!";
            }

            return response;
        }

        public async Task<BaseResponseModel> Edit(AspiceProcessModel request)
        {
            BaseResponseModel response = new BaseResponseModel();

            if (request.Validate)
            {
                if (await CheckAspiceVersion(request.AspiceVersionId, response))
                {
                    AspiceProcess aspiceProcess = await Load(request.Id, response);
                    if (aspiceProcess != null)
                    {
                        aspiceProcess.Name = request.Name;
                        aspiceProcess.Shortcut = request.Shortcut;
                        aspiceProcess.Description = request.Description;
                        aspiceProcess.AspiceVersionId = request.AspiceVersionId;

                        await Database.SaveChangesAsync();

                        response.Message = "Automotive SPICE process was successfully edited!";
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

            AspiceProcess aspiceProcess = await Load(id, response);
            if (aspiceProcess != null)
            {
                Database.AspiceProcess.Remove(aspiceProcess);

                await Database.SaveChangesAsync();

                response.Message = "Automotive SPICE process was successfully deleted!";
            }

            return response;
        }

        public async Task<AspiceProcess> Load(int id, BaseResponseModel response)
        {
            AspiceProcess aspiceProcess = await Database.AspiceProcess.FirstOrDefaultAsync(a => a.Id == id);
            if (aspiceProcess == null)
            {
                response.Success = false;
                response.Message = "Unknown Automotive SPICE process!";
            }

            return aspiceProcess;
        }

        private async Task<AspiceVersionModel> GetAspiceVersion(int aspiceVersionId) => await _aspiceVersionService.Get(aspiceVersionId);

        private async Task<bool> CheckAspiceVersion(int aspiceVersionId, BaseResponseModel response)
        {
            if (await Database.AspiceVersion.AnyAsync(a => a.Id == aspiceVersionId))
            {
                return true;
            }
            else
            {
                response.Success = false;
                response.Message = "Unknown Automotive SPICE version!";

                return false;
            }
        }
    }
}
