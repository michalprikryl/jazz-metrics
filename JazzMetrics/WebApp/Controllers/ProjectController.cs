using Library.Models.ProjectMetrics;
using Library.Models.Projects;
using Library.Models.ProjectUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Project;
using WebApp.Models.Project.Dashboard;
using WebApp.Models.Project.ProjectMetric;
using WebApp.Models.Project.ProjectUser;
using WebApp.Services.Crud;
using WebApp.Services.Error;
using WebApp.Services.Project;
using WebApp.Services.Users;

namespace WebApp.Controllers
{
    [Route("Project")]
    public class ProjectController : AppController
    {
        private readonly ICrudService _crudService;
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;

        public ProjectController(IErrorService errorService, ICrudService crudService, IUserService userService, IProjectService projectService) : base(errorService)
        {
            _crudService = crudService;
            _userService = userService;
            _projectService = projectService;
        }

        #region Project
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ProjectListModel model = new ProjectListModel();

            var result = await _crudService.GetAll<ProjectModel>(Token, ProjectService.ProjectEntity, false);
            if (result.Success)
            {
                model.Projects = result.Values.Select(p =>
                    new ProjectViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        CreateDate = p.CreateDate,
                        Description = p.Description,
                        ProjectUsersCount = p.ProjectUsers.Count,
                        ProjectMetricsCount = p.ProjectMetrics.Count
                    }).ToList();
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View(model);
        }

        [HttpGet("Add")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public IActionResult ProjectAdd()
        {
            ProjectWorkModel model = new ProjectWorkModel();

            return View("Add", model);
        }

        [HttpPost("Add")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> ProjectAddPost(ProjectWorkModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _crudService.Create(model, Token, ProjectService.ProjectEntity);

                AddMessageToModel(model, result.Message, !result.Success);
            }
            else
            {
                AddModelStateErrors(model);
            }

            return View("Add", model);
        }

        [HttpGet("Edit/{id}")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> ProjectEdit(int id)
        {
            ProjectWorkModel model = new ProjectWorkModel();

            var result = await _crudService.Get<ProjectModel>(id, Token, ProjectService.ProjectEntity);
            if (result.Success)
            {
                model.Id = id;
                model.Name = result.Value.Name;
                model.Description = result.Value.Description;
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("Edit", model);
        }

        [HttpPost("Edit/{id}")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> ProjectEditPost(int id, ProjectWorkModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _crudService.Edit(id, model, Token, ProjectService.ProjectEntity);

                AddMessageToModel(model, result.Message, !result.Success);
            }
            else
            {
                AddModelStateErrors(model);
            }

            return View("Edit", model);
        }

        [HttpPost("Delete/{id}")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> ProjectDelete(int id)
        {
            return Json(await _crudService.Drop(id, Token, ProjectService.ProjectEntity));
        }
        #endregion

        #region Projects dashboard
        [HttpGet("{id}/Dashboard")]
        public async Task<IActionResult> Dashboard(int id)
        {
            ProjectDashboardViewModel model = new ProjectDashboardViewModel();

            var result = await _crudService.Get<ProjectModel>(id, Token, ProjectService.ProjectEntity, false);
            if (result.Success)
            {
                ProjectModel value = result.Value;

                model.Id = value.Id;
                model.Name = value.Name;
                model.Description = value.Description;
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("Dashboard/Index", model);
        }
        #endregion

        #region Project's users
        [HttpGet("{id}/User")]
        public async Task<IActionResult> ProjectUsers(int id)
        {
            ProjectUserListModel model = new ProjectUserListModel
            {
                User = new ProjectUserWorkModel { ProjectId = id }
            };

            CheckTempData(model);

            var result = await _crudService.Get<ProjectModel>(id, Token, ProjectService.ProjectEntity, false);
            if (result.Success)
            {
                model.Id = result.Value.Id;
                model.Name = result.Value.Name;
                model.Users = result.Value.ProjectUsers.Select(p =>
                    new ProjectUserViewModel
                    {
                        UserId = p.User.Id,
                        JoinDate = p.JoinDate,
                        Username = p.User.Username,
                        UserInfo = p.User.ToString(),
                        Admin = p.User.Admin
                    }).ToList();
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("User/Index", model);
        }

        [HttpPost("User/Add")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> ProjectUserAdd(ProjectUserWorkModel model)
        {
            ViewModel viewModel = new ViewModel();

            if (ModelState.IsValid)
            {
                var userResult = await _userService.FindUserIdByUsername(model.Username, Token);
                if (userResult.Success)
                {
                    var result = await _crudService.Create(new ProjectUserModel { ProjectId = model.ProjectId, UserId = userResult.Id }, Token, ProjectService.ProjectUserEntity);

                    AddMessageToModel(viewModel, result.Message, !result.Success);
                }
                else
                {
                    AddMessageToModel(viewModel, userResult.Message);
                }
            }
            else
            {
                AddModelStateErrors(viewModel);
            }

            AddViewModelToTempData(viewModel);

            return RedirectToAction("ProjectUsers", new { id = model.ProjectId });
        }

        [HttpPost("User/Delete")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> ProjectUserDelete([FromBody]ProjectUserModel model)
        {
            var result = await _projectService.GetProjectUser(model.UserId, model.ProjectId, Token);
            if (result.Success)
            {
                return Json(await _crudService.Drop(result.Value.Id, Token, ProjectService.ProjectUserEntity));

            }
            else
            {
                return Json(result);
            }
        }
        #endregion

        #region Project's metrics
        [HttpGet("{id}/Metric")]
        public async Task<IActionResult> ProjectMetrics(int id)
        {
            ProjectMetricListModel model = new ProjectMetricListModel();

            CheckTempData(model);

            var result = await _crudService.Get<ProjectModel>(id, Token, ProjectService.ProjectEntity, false);
            if (result.Success)
            {
                model.Id = result.Value.Id;
                model.Name = result.Value.Name;
                model.Metrics = result.Value.ProjectMetrics.Select(m =>
                    new ProjectMetricViewModel
                    {
                        CreateDate = m.CreateDate,
                        LastUpdateDate = m.LastUpdateDate,
                        MetricId = m.MetricId,
                        MetricInfo = m.Metric.ToString(),
                        ProjectMetricId = m.Id,
                        Public = m.Metric.Public,
                        Warning = m.Warning,
                        CanEdit = m.Metric.CompanyId == MyUser.CompanyId
                    }).ToList();
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View("Metric/Index", model);
        }

        [HttpGet("{id}/Metric/Add")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> ProjectMetricAdd(int id)
        {
            ProjectMetricWorkModel model = new ProjectMetricWorkModel { ProjectId = id };

            await GetMetrics(model);

            return View("Metric/Add", model);
        }

        [HttpPost("{id}/Metric/Add")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> ProjectMetricAdd(int id, ProjectMetricWorkModel model)
        {
            Task task = GetMetrics(model);

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.DataPassword))
                {
                    model.ProjectId = id;

                    var result = await _crudService.Create(model, Token, ProjectService.ProjectMetricEntity);

                    AddMessageToModel(model, result.Message, !result.Success);
                }
                else
                {
                    AddMessageToModel(model, "Paste your password, please!");
                }
            }
            else
            {
                AddModelStateErrors(model);
            }

            await Task.WhenAll(task);

            return View("Metric/Add", model);
        }

        [HttpGet("{projectId}/Metric/Edit/{id}")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> ProjectMetricEdit(int projectId, int id)
        {
            ProjectMetricWorkModel model = new ProjectMetricWorkModel();

            Task task = GetMetrics(model);

            var result = await _crudService.Get<ProjectMetricModel>(id, Token, ProjectService.ProjectMetricEntity);
            if (result.Success)
            {
                model.Id = id;
                model.ProjectId = projectId;
                model.DataUrl = result.Value.DataUrl;
                model.DataUsername = result.Value.DataUsername;
                model.DataPassword = result.Value.DataPassword;
                model.MetricId = result.Value.MetricId.ToString();
                model.Warning = result.Value.Warning;
                model.MinimalWarningValue = result.Value.MinimalWarningValue;
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            await Task.WhenAll(task);

            return View("Metric/Edit", model);
        }

        [HttpPost("{projectId}/Metric/Edit/{id}")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> ProjectMetricEdit(int projectId, int id, ProjectMetricWorkModel model)
        {
            Task task = GetMetrics(model);

            if (ModelState.IsValid)
            {
                model.Id = id;
                model.ProjectId = projectId;

                var result = await _crudService.Edit(id, model, Token, ProjectService.ProjectMetricEntity);

                AddMessageToModel(model, result.Message, !result.Success);
            }
            else
            {
                AddModelStateErrors(model);
            }

            await Task.WhenAll(task);

            return View("Metric/Edit", model);
        }

        [HttpPost("{projectId}/Metric/Delete/{id}")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<IActionResult> ProjectMetricDelete(int projectId, int id)
        {
            return Json(await _crudService.Drop(id, Token, ProjectService.ProjectMetricEntity));
        }

        private async Task GetMetrics(ProjectMetricWorkModel model)
        {
            model.Metrics = await _projectService.GetMetricsForSelect(Token);

            if (model.Metrics == null)
            {
                AddMessageToModel(model, "Cannot retrieve metrics, press F5 please.");
            }
            else if (model.Metrics.Count == 0)
            {
                AddMessageToModel(model, "Your company doesn't have any metrics, please create at least one.");
            }
        }
        #endregion
    }
}