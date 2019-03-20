using Database;
using Database.DAO;
using Library;
using Library.Models;
using Library.Models.ProjectMetrics;
using Library.Models.Projects;
using Library.Models.ProjectUsers;
using Library.Networking;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Controllers;
using WebAPI.Services.Helper;
using WebAPI.Services.Helpers;
using WebAPI.Services.Metrics;
using WebAPI.Services.ProjectMetrics;
using WebAPI.Services.ProjectUsers;
using WebAPI.Services.Users;

namespace WebAPI.Services.Projects
{
    public class ProjectService : BaseDatabase, IProjectService
    {
        private readonly IUserService _userService;
        private readonly IMetricService _metricService;
        private readonly IProjectUserService _projectUserService;
        private readonly IProjectMetricService _projectMetricService;

        public CurrentUser CurrentUser { get; set; }

        public ProjectService(JazzMetricsContext db, IUserService userService, IProjectMetricService projectMetricService, IProjectUserService projectUserService,
            IHelperService helperService, IHttpContextAccessor contextAccessor, IMetricService metricService) : base(db)
        {
            _userService = userService;
            _metricService = metricService;
            _projectUserService = projectUserService;
            _projectMetricService = projectMetricService;
            CurrentUser = helperService.GetCurrentUser(contextAccessor.HttpContext.User.GetId());
        }

        public ProjectModel ConvertToModel(Project dbModel)
        {
            return new ProjectModel
            {
                Id = dbModel.Id,
                Name = dbModel.Name,
                CreateDate = dbModel.CreateDate,
                Description = dbModel.Description
            };
        }

        public async Task<BaseResponseModelPost> Create(ProjectModel request)
        {
            BaseResponseModelPost response = new BaseResponseModelPost();

            if (request.Validate())
            {
                Project project = new Project
                {
                    Name = request.Name,
                    Description = request.Description,
                    CreateDate = DateTime.Now
                };

                await Database.Project.AddAsync(project);

                ProjectUser userProject = new ProjectUser
                {
                    JoinDate = DateTime.Now,
                    Project = project,
                    UserId = CurrentUser.Id
                };

                await Database.ProjectUser.AddAsync(userProject);

                await Database.SaveChangesAsync();

                response.Id = project.Id;
                response.Message = "Project was successfully created!";
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

            Project project = await Load(id, response);
            if (project != null)
            {
                if (project.ProjectMetric.Count == 0)
                {
                    Database.ProjectUser.RemoveRange(project.ProjectUser);

                    Database.Project.Remove(project);

                    await Database.SaveChangesAsync();

                    response.Message = "Project was successfully deleted!";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Cannot delete project, because this project defined has metrics!";
                }
            }

            return response;
        }

        public async Task<BaseResponseModel> Edit(ProjectModel request)
        {
            BaseResponseModel response = new BaseResponseModel();

            if (request.Validate())
            {
                Project project = await Load(request.Id, response);
                if (project != null)
                {
                    project.Name = request.Name;
                    project.Description = request.Description;

                    await Database.SaveChangesAsync();

                    response.Message = "Project was successfully edited!";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Some of the required properties is not present!";
            }

            return response;
        }

        public async Task<BaseResponseModelGet<ProjectModel>> Get(int id, bool lazy)
        {
            var response = new BaseResponseModelGet<ProjectModel>();

            Project project = await Load(id, response);
            if (project != null)
            {
                response.Value = ConvertToModel(project);

                if (!lazy)
                {
                    response.Value.ProjectUsers = GetProjectUsers(project.ProjectUser);
                    response.Value.ProjectMetrics = GetProjectMetrics(project.ProjectMetric);
                }
            }

            return response;
        }

        public async Task<BaseResponseModelGetAll<ProjectModel>> GetAll(bool lazy)
        {
            var response = new BaseResponseModelGetAll<ProjectModel> { Values = new List<ProjectModel>() };

            foreach (var item in await LoadUsersProjects())
            {
                ProjectModel project = ConvertToModel(item);

                if (!lazy)
                {
                    project.ProjectUsers = GetProjectUsers(item.ProjectUser);
                    project.ProjectMetrics = GetProjectMetrics(item.ProjectMetric);
                }

                response.Values.Add(project);
            }

            return response;
        }

        public async Task<Project> Load(int id, BaseResponseModel response)
        {
            Project project = await Database.Project.FirstOrDefaultAsync(a => a.Id == id);
            if (project == null)
            {
                response.Success = false;
                response.Message = "Unknown project!";
            }
            else
            {
                User user = await Database.User.FirstAsync(u => u.Id == CurrentUser.Id);
                if ((user.UserRole.Name == MainController.RoleUser && project.ProjectUser.All(p => p.UserId != CurrentUser.Id)) ||
                    (user.UserRole.Name != MainController.RoleUser && project.ProjectUser.All(u => u.User.CompanyId != user.CompanyId)))
                {
                    project = null;
                    response.Success = false;
                    response.Message = "You don't have access to this project!";
                }
            }

            return project;
        }

        public async Task<IEnumerable<Project>> LoadUsersProjects()
        {
            User user = await Database.User.FirstAsync(u => u.Id == CurrentUser.Id);
            if (user.UserRole.Name == MainController.RoleUser)
            {
                return user.ProjectUser.Select(u => u.Project);
            }
            else
            {
                return (await Database.Project.ToListAsync()).Where(p => p.ProjectUser.Any(u => u.User.CompanyId == user.CompanyId));
            }
        }

        public Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            throw new NotImplementedException();
        }

        private List<ProjectUserModel> GetProjectUsers(ICollection<ProjectUser> users) => users.Select(u =>
            {
                ProjectUserModel projectUser = _projectUserService.ConvertToModel(u);
                projectUser.User = _userService.ConvertToModel(u.User);
                return projectUser;
            }).ToList();

        private List<ProjectMetricModel> GetProjectMetrics(ICollection<ProjectMetric> metrics) => metrics.Select(m =>
            {
                ProjectMetricModel projectMetric = _projectMetricService.ConvertToModel(m);
                projectMetric.Metric = _metricService.ConvertToModel(m.Metric);
                return projectMetric;
            }).ToList();
    }
}
