using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Project;
using WebApp.Services.Crud;
using WebApp.Services.Error;
using WebApp.Services.Project;

namespace WebApp.Controllers
{
    public class ProjectController : AppController
    {
        private readonly ICrudService _crudService;

        public ProjectController(IErrorService errorService, ICrudService crudService) : base(errorService)
        {
            _crudService = crudService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ProjectListModel model = new ProjectListModel();

            var result = await _crudService.GetAll<ProjectModel>(Token, ProjectService.ProjectEntity);
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
    }
}