using Library.Models.Projects;
using Library.Models.ProjectUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Project;
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

        #region Project's users
        [HttpGet("User/{id}")]
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
    }
}