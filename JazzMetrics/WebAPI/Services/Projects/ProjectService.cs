using Database;
using Database.DAO;
using Library;
using Library.Jazz;
using Library.Models;
using Library.Models.Error;
using Library.Models.ProjectMetrics;
using Library.Models.ProjectMetricSnapshots;
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
using WebAPI.Services.MetricTypes;
using WebAPI.Services.ProjectMetrics;
using WebAPI.Services.ProjectMetricSnapshots;
using WebAPI.Services.ProjectUsers;
using WebAPI.Services.Users;

namespace WebAPI.Services.Projects
{
    public class ProjectService : BaseDatabase, IProjectService
    {
        private readonly IJazzService _jazzService;
        private readonly IUserService _userService;
        private readonly IHelperService _helperService;
        private readonly IMetricService _metricService;
        private readonly IMetricTypeService _metricTypeService;
        private readonly IProjectUserService _projectUserService;
        private readonly IProjectMetricService _projectMetricService;
        private readonly IProjectMetricSnapshotService _projectMetricSnapshotService;

        public CurrentUser CurrentUser { get; set; }

        public ProjectService(JazzMetricsContext db, IUserService userService, IProjectMetricService projectMetricService, IProjectUserService projectUserService,
            IHelperService helperService, IHttpContextAccessor contextAccessor, IMetricService metricService, IJazzService jazzService, IProjectMetricSnapshotService projectMetricSnapshotService,
            IMetricTypeService metricTypeService) : base(db)
        {
            _jazzService = jazzService;
            _userService = userService;
            _helperService = helperService;
            _metricService = metricService;
            _metricTypeService = metricTypeService;
            _projectUserService = projectUserService;
            _projectMetricService = projectMetricService;
            _projectMetricSnapshotService = projectMetricSnapshotService;
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

        public async Task<BaseResponseModelGet<ProjectModel>> Get(int id)
        {
            var response = new BaseResponseModelGet<ProjectModel>();

            Project project = await Load(id, response);
            if (project != null)
            {
                response.Value = ConvertToModel(project);

                response.Value.ProjectMetrics = GetProjectMetrics(project.ProjectMetric);

                for (int i = 0; i < response.Value.ProjectMetrics.Count; i++)
                {
                    response.Value.ProjectMetrics[i].Snapshots = GetProjectMetricSnapshots(project.ProjectMetric.ElementAt(i).ProjectMetricSnapshot);
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

        public async Task<BaseResponseModel> CreateSnapshots()
        {
            BaseResponseModel response = new BaseResponseModel { Message = "Update of all projects metrics ended successfully. For more detailed result check project metric log." };

            List<Task> tasks = new List<Task>();
            foreach (var item in Database.Project)
            {
                tasks.Add(CreateSnapshots(item.Id, item));
            }

            await Task.WhenAll(tasks);

            return response;
        }

        public async Task<BaseResponseModel> CreateSnapshots(int id, Project project = null)
        {
            BaseResponseModel response = new BaseResponseModel { Message = "Update of all project metrics ended successfully. For more detailed result check project metric log." };

            project = project ?? await Load(id, response);
            if (project != null)
            {
                foreach (var item in project.ProjectMetric)
                {
                    await CreateSnapshot(project.Id, item.Id, item);
                }
            }

            return response;
        }

        public async Task<BaseResponseModel> CreateSnapshot(int id, int projectMetricId, ProjectMetric projectMetric = null)
        {
            BaseResponseModel response = new BaseResponseModel { Message = "Creating of a snapshot ended successfully. For more detailed result check project metric log." };

            try
            {
                projectMetric = projectMetric ?? await _projectMetricService.Load(projectMetricId, response);
                if (projectMetric != null)
                {
                    await _jazzService.CreateSnapshot(projectMetric);

                    await Database.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = "Creating of a snapshot was not successfull! Please try again later.";
                await _helperService.SaveErrorToDB(new ErrorModel(e, message: "task - all projects", module: "ProjectService", function: "CreateSnapshots"));
                projectMetric.ProjectMetricLog.Add(new ProjectMetricLog($"Within processing of snapshot for project metric #{projectMetric.Id} occured and error!"));
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
                projectMetric.Metric.Columns = _metricService.GetMetricColumns(m.Metric.MetricColumn);
                projectMetric.Metric.MetricType = _metricTypeService.ConvertToModel(m.Metric.MetricType);
                return projectMetric;
            }).ToList();

        private List<ProjectMetricSnapshotModel> GetProjectMetricSnapshots(ICollection<ProjectMetricSnapshot> snapshots) => snapshots.Select(s => _projectMetricSnapshotService.ConvertToModel(s)).ToList();
    }
}
