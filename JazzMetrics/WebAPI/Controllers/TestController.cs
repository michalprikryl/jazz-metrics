using Library.Models.Test;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Helper;
using WebAPI.Services.Test;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : MainController
    {
        private readonly ITestService _testService;

        public TestController(IHelperService helperService, ITestService testService) : base(helperService)
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