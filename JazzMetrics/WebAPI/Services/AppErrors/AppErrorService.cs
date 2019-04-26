using Database;
using Database.DAO;
using Library;
using Library.Models;
using Library.Models.AppError;
using Library.Networking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.AppErrors
{
    /// <summary>
    /// servis pro praci s DB tabulkou AppError
    /// </summary>
    public class AppErrorService : BaseDatabase, IAppErrorService
    {
        /// <summary>
        /// pro zapis do DB
        /// </summary>
        public const string UNKNOWN = "unknown";

        public AppErrorService(JazzMetricsContext db) : base(db) { }

        public AppErrorModel ConvertToModel(AppError dbModel)
        {
            return new AppErrorModel
            {
                Exception = dbModel.Exception,
                Function = dbModel.Function,
                InnerException = dbModel.InnerException,
                Message = dbModel.Message,
                Module = dbModel.Module,
                Time = dbModel.Time,
                AppInfo = dbModel.AppInfo,
                Deleted = dbModel.Deleted,
                Id = dbModel.Id,
                Solved = dbModel.Solved
            };
        }

        public async Task<BaseResponseModelPost> Create(AppErrorModel request)
        {
            BaseResponseModelPost response = new BaseResponseModelPost();

            AppError appError = new AppError
            {
                Deleted = false,
                Exception = request.Exception ?? UNKNOWN,
                Function = request.Function ?? UNKNOWN,
                InnerException = request.InnerException ?? UNKNOWN,
                Message = request.Message ?? UNKNOWN,
                Module = request.Module ?? UNKNOWN,
                Solved = false,
                Time = request.Time ?? DateTime.Now,
                AppInfo = request.AppInfo ?? UNKNOWN
            };

            await Database.AppError.AddAsync(appError);

            await Database.SaveChangesAsync();

            response.Id = appError.Id;
            response.Message = "App error was successfully created!";

            return response;
        }

        public async Task<BaseResponseModel> Drop(int id)
        {
            BaseResponseModel response = new BaseResponseModel();

            AppError appError = await Load(id, response);
            if (appError != null)
            {
                appError.Deleted = true;

                await Database.SaveChangesAsync();

                response.Message = "App error was successfully deleted!";
            }

            return response;
        }

        public Task<BaseResponseModel> Edit(AppErrorModel request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModelGet<AppErrorModel>> Get(int id, bool lazy)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponseModelGetAll<AppErrorModel>> GetAll(bool lazy)
        {
            return new BaseResponseModelGetAll<AppErrorModel>
            {
                Values = await Database.AppError.Select(a => ConvertToModel(a)).ToListAsyncSpecial()
            };
        }

        public async Task<AppError> Load(int id, BaseResponseModel response, bool tracking = true, bool lazy = true)
        {
            AppError appError = await Database.AppError.FirstOrDefaultAsync(a => a.Id == id);
            if (appError == null)
            {
                response.Success = false;
                response.Message = "Unknown app error!";
            }

            return appError;
        }

        public async Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            BaseResponseModel response = new BaseResponseModel();

            AppError appError = await Load(id, response);
            if (appError != null)
            {
                foreach (var item in request)
                {
                    if (string.Compare(item.PropertyName, "solved", true) == 0)
                    {
                        appError.Solved = Convert.ToBoolean(item.Value);
                    }
                    else if (string.Compare(item.PropertyName, "deleted", true) == 0)
                    {
                        appError.Deleted = Convert.ToBoolean(item.Value);
                    }
                }

                await Database.SaveChangesAsync();

                response.Message = "App error was successfully edited!";
            }

            return response;
        }
    }
}
