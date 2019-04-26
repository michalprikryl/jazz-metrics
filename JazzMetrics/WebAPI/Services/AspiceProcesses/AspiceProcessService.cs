using Database;
using Database.DAO;
using Library;
using Library.Models;
using Library.Models.AspiceProcesses;
using Library.Models.AspiceVersions;
using Library.Networking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebAPI.Services.AspiceVersions;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.AspiceProcesses
{
    /// <summary>
    /// servis pro praci s DB tabulkou AffectedField
    /// </summary>
    public class AspiceProcessService : BaseDatabase, IAspiceProcessService
    {
        /// <summary>
        /// servis pro praci s AspiceVersion
        /// </summary>
        private readonly IAspiceVersionService _aspiceVersionService;

        public AspiceProcessService(JazzMetricsContext db, IAspiceVersionService aspiceVersionService) : base(db) => _aspiceVersionService = aspiceVersionService;

        public async Task<BaseResponseModelGetAll<AspiceProcessModel>> GetAll(bool lazy)
        {
            var response = new BaseResponseModelGetAll<AspiceProcessModel> { Values = new List<AspiceProcessModel>() };

            foreach (var item in await Database.AspiceProcess.ToListAsyncSpecial(a => a.AspiceVersion))
            {
                AspiceProcessModel aspiceProcess = ConvertToModel(item);

                if (!lazy)
                {
                    aspiceProcess.AspiceVersion = GetAspiceVersion(item.AspiceVersion);
                }

                response.Values.Add(aspiceProcess);
            }

            return response;
        }

        public async Task<BaseResponseModelGet<AspiceProcessModel>> Get(int id, bool lazy)
        {
            var response = new BaseResponseModelGet<AspiceProcessModel>();

            AspiceProcess aspiceProcess = await Load(id, response, false, lazy);
            if (aspiceProcess != null)
            {
                response.Value = ConvertToModel(aspiceProcess);

                if (!lazy)
                {
                    response.Value.AspiceVersion = GetAspiceVersion(aspiceProcess.AspiceVersion);
                }
            }

            return response;
        }

        public async Task<BaseResponseModelPost> Create(AspiceProcessModel request)
        {
            BaseResponseModelPost response = new BaseResponseModelPost();

            if (request.Validate())
            {
                if (await CheckAspiceVersion(request.AspiceVersionId, response))
                {
                    AspiceProcess aspiceProcess = new AspiceProcess
                    {
                        Name = request.Name,
                        Shortcut = request.Shortcut,
                        Description = request.Description,
                        AspiceVersionId = request.AspiceVersionId
                    };

                    await Database.AspiceProcess.AddAsync(aspiceProcess);

                    await Database.SaveChangesAsync();

                    response.Id = aspiceProcess.Id;
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

            if (request.Validate())
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
                if (aspiceProcess.Metric.Count == 0)
                {
                    Database.AspiceProcess.Remove(aspiceProcess);

                    await Database.SaveChangesAsync();

                    response.Message = "Automotive SPICE process was successfully deleted!";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Cannot delete Automotive SPICE process, because some metrics use this process!";
                }
            }

            return response;
        }

        public Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AspiceProcess> Load(int id, BaseResponseModel response, bool tracking = true, bool lazy = true)
        {
            AspiceProcess aspiceProcess = await Database.AspiceProcess.FirstOrDefaultAsyncSpecial(a => a.Id == id, tracking, !lazy ? (Expression<Func<AspiceProcess, AspiceVersion>>)(a => a.AspiceVersion) : null);
            if (aspiceProcess == null)
            {
                response.Success = false;
                response.Message = "Unknown Automotive SPICE process!";
            }

            return aspiceProcess;
        }

        private AspiceVersionModel GetAspiceVersion(AspiceVersion aspiceVersion) => _aspiceVersionService.ConvertToModel(aspiceVersion);

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

        public AspiceProcessModel ConvertToModel(AspiceProcess dbModel)
        {
            return new AspiceProcessModel
            {
                Id = dbModel.Id,
                Name = dbModel.Name,
                Shortcut = dbModel.Shortcut,
                Description = dbModel.Description,
                AspiceVersionId = dbModel.AspiceVersionId
            };
        }
    }
}
