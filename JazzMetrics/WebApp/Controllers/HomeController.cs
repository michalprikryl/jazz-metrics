using Library.Models.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Home;
using WebApp.Services.Crud;
using WebApp.Services.Error;
using WebApp.Services.Project;

namespace WebApp.Controllers
{
    public class HomeController : AppController
    {
        private readonly ICrudService _crudService;

        public HomeController(IErrorService errorService, ICrudService crudService) : base(errorService) => _crudService = crudService;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel();

            var result = await _crudService.GetAll<ProjectModel>(Token, ProjectService.ProjectEntity, false);
            if (result.Success)
            {
                model.Projects = result.Values.Select(p =>
                    new HomeProjectModel
                    {
                        ProjectId = p.Id,
                        ProjectName = p.Name,
                        ProjectDescription = p.Description
                    }).ToList();
            }
            else
            {
                AddMessageToModel(model, result.Message);
            }

            return View(model);
        }

        /// <summary>
        /// test zachyceni chyby
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult About()
        {
            int[] u = new int[] { 1 };
            u[51] = 10;

            ViewData["Message"] = "Your application description page.";

            return View();
        }
    }
}
