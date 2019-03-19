using Database;
using Database.DAO;
using Library.Models;
using Library.Models.ProjectUsers;
using Library.Networking;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Controllers;
using WebAPI.Services.Helper;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.ProjectUsers
{
    public class ProjectUserService : BaseDatabase, IProjectUserService
    {
        public CurrentUser CurrentUser { get; set; }

        public ProjectUserService(JazzMetricsContext db, IHelperService helperService, IHttpContextAccessor contextAccessor) : base(db) =>
            CurrentUser = helperService.GetCurrentUser(contextAccessor.HttpContext.User.GetId());

        public ProjectUserModel ConvertToModel(ProjectUser dbModel)
        {
            return new ProjectUserModel
            {
                Id = dbModel.Id,
                JoinDate = dbModel.JoinDate,
                UserId = dbModel.UserId,
                ProjectId = dbModel.ProjectId
            };
        }

        public async Task<BaseResponseModelGet<ProjectUserModel>> GetByProjectAndUser(int projectId, int userId)
        {
            var response = new BaseResponseModelGet<ProjectUserModel>();

            ProjectUser projectUser = await Database.ProjectUser.FirstOrDefaultAsync(p => p.ProjectId == projectId && p.UserId == userId);
            if (projectUser != null)
            {
                response.Value = ConvertToModel(projectUser);
            }
            else
            {
                response.Success = false;
                response.Message = "User is not member of this project!";
            }

            return response;
        }

        public async Task<BaseResponseModelPost> Create(ProjectUserModel request)
        {
            BaseResponseModelPost response = new BaseResponseModelPost();

            if (await CheckUser(request.UserId, response) && await CheckProject(request.ProjectId, response))
            {
                if (await Database.ProjectUser.AllAsync(p => p.ProjectId == request.ProjectId && p.UserId != request.UserId))
                {
                    ProjectUser projectUser =
                        new ProjectUser
                        {
                            JoinDate = DateTime.Now,
                            UserId = request.UserId,
                            ProjectId = request.ProjectId
                        };

                    await Database.ProjectUser.AddAsync(projectUser);

                    await Database.SaveChangesAsync();

                    response.Id = projectUser.Id;
                    response.Message = "User was successfully added to project!";
                }
                else
                {
                    response.Success = false;
                    response.Message = "User is already member of this project!";
                }
            }

            return response;
        }

        public async Task<BaseResponseModel> Drop(int id)
        {
            BaseResponseModel response = new BaseResponseModel();

            ProjectUser projectUser = await Load(id, response);
            if (projectUser != null)
            {
                if (projectUser.User.UserRole.Name == MainController.RoleAdmin)
                {
                    projectUser.User.UserRole = await Database.UserRole.FirstAsync(r => r.Name == MainController.RoleUser);
                }

                Database.ProjectUser.Remove(projectUser);

                await Database.SaveChangesAsync();

                response.Message = "User was successfully deleted from project!";
            }

            return response;
        }

        public Task<BaseResponseModel> Edit(ProjectUserModel request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModelGet<ProjectUserModel>> Get(int id, bool lazy)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModelGetAll<ProjectUserModel>> GetAll(bool lazy)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectUser> Load(int id, BaseResponseModel response)
        {
            ProjectUser projectUser = await Database.ProjectUser.FirstOrDefaultAsync(a => a.Id == id && a.User.CompanyId.HasValue && a.User.CompanyId == CurrentUser.CompanyId);
            if (projectUser == null)
            {
                response.Success = false;
                response.Message = "Unknown connection between user and project!";
            }

            return projectUser;
        }

        public Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> CheckUser(int userId, BaseResponseModel response)
        {
            if (await Database.User.AnyAsync(a => a.Id == userId && a.CompanyId.HasValue && a.CompanyId == CurrentUser.CompanyId))
            {
                return true;
            }
            else
            {
                response.Success = false;
                response.Message = "Unknown user!";

                return false;
            }
        }

        private async Task<bool> CheckProject(int projectId, BaseResponseModel response)
        {
            if (await Database.Project.AnyAsync(a => a.Id == projectId))
            {
                return true;
            }
            else
            {
                response.Success = false;
                response.Message = "Unknown project!";

                return false;
            }
        }
    }
}
