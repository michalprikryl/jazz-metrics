using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.DAO;
using Library.Networking;
using Microsoft.EntityFrameworkCore;
using WebAPI.Controllers;
using WebAPI.Models;
using WebAPI.Models.ProjectMetrics;
using WebAPI.Models.Projects;
using WebAPI.Models.Users;
using WebAPI.Services.Helpers;
using WebAPI.Services.ProjectMetrics;
using WebAPI.Services.Users;

namespace WebAPI.Services.Projects
{
    public class ProjectService : BaseDatabase, IProjectService
    {
        private readonly IUserService _userService;
        private readonly IProjectMetricService _projectMetricService;

        public int CurrentUserId { get; set; }

        public ProjectService(JazzMetricsContext db, IUserService userService, IProjectMetricService projectMetricService) : base(db)
        {
            _userService = userService;
            _projectMetricService = projectMetricService;
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

            if (request.Validate)
            {
                Project project = new Project
                {
                    Name = request.Name,
                    Description = request.Description,
                    CreateDate = DateTime.Now
                };

                await Database.Project.AddAsync(project);

                UserProject userProject = new UserProject
                {
                    JoinDate = DateTime.Now,
                    Project = project,
                    UserId = CurrentUserId
                };

                await Database.UserProject.AddAsync(userProject);

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

        public async Task<BaseResponseModel> DropAsync(int id)
        {
            BaseResponseModel response = new BaseResponseModel();

            Project project = await Load(id, response);
            if (project != null)
            {
                if (project.ProjectMetric.Count == 0)
                {
                    Database.UserProject.RemoveRange(project.UserProject);

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

            if (request.Validate)
            {
                Project project = await Load(request.Id, response);
                if (project != null)
                {
                    project.Name = request.Name;
                    project.Description = request.Description; ;

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

        public async Task<ProjectModel> Get(int id, bool lazy)
        {
            ProjectModel response = new ProjectModel();
            BaseResponseModel result = new BaseResponseModel();

            Project project = await Load(id, result);
            if (project != null)
            {
                response = ConvertToModel(project);

                if (!lazy)
                {
                    response.ProjectUsers = GetProjectUsers(project.UserProject);
                    response.ProjectMetrics = GetProjectMetrics(project.ProjectMetric);
                }
            }
            else
            {
                response.Success = result.Success;
                response.Message = result.Message;
            }

            return response;
        }

        public async Task<BaseResponseModelGet<ProjectModel>> GetAll(bool lazy)
        {
            var response = new BaseResponseModelGet<ProjectModel> { Values = new List<ProjectModel>() };

            foreach (var item in await LoadUsersProjects())
            {
                ProjectModel project = ConvertToModel(item);

                if (!lazy)
                {
                    project.ProjectUsers = GetProjectUsers(item.UserProject);
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
                User user = await Database.User.FirstAsync(u => u.Id == CurrentUserId);
                if ((user.UserRole.Name == MainController.RoleUser && project.UserProject.All(p => p.UserId != CurrentUserId)) ||
                    (user.UserRole.Name != MainController.RoleUser && project.UserProject.All(u => u.User.CompanyId != user.CompanyId)))
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
            User user = await Database.User.FirstAsync(u => u.Id == CurrentUserId);
            if (user.UserRole.Name == MainController.RoleUser)
            {
                return user.UserProject.Select(u => u.Project);
            }
            else
            {
                return (await Database.Project.ToListAsync()).Where(p => p.UserProject.Any(u => u.User.CompanyId == user.CompanyId));
            }
        }

        public Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            throw new NotImplementedException();
        }

        private List<UserModel> GetProjectUsers(ICollection<UserProject> users) => users.Select(u => _userService.ConvertToModel(u.User)).ToList();

        private List<ProjectMetricModel> GetProjectMetrics(ICollection<ProjectMetric> metrics) => metrics.Select(m => _projectMetricService.ConvertToModel(m)).ToList();
    }
}
