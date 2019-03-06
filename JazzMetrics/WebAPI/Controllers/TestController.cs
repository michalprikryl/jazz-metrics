using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Test;
using WebAPI.Services.Error;
using WebAPI.Services.Test;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : MainController
    {
        private readonly ITestService _testService;

        public TestController(IErrorService errorService, ITestService testService) : base(errorService)
        {
            _testService = testService;
        }

        [HttpGet, AllowAnonymous]
        public ActionResult<TestModel> Get()
        {
            return _testService.RunTest();
        }
    }
}