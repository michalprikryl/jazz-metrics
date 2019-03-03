using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Models.Test;
using WebApp.Services.Error;
using WebApp.Services.Test;

namespace WebApp.Controllers
{
    public class TestController : AppController
    {
        private readonly ITestService _testService;

        public TestController(IErrorService errorService, ITestService testService) : base(errorService) => _testService = testService;

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            TestViewModel model = await _testService.TestConnectionToAPI();

            return View(model);
        }
    }
}
