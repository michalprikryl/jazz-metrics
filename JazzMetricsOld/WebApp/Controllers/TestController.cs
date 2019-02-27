using WebApp.Classes.Test;
using WebApp.Models.Test;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class TestController : AppController
    {
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            TestConnection test = new TestConnection();
            TestResult result = await test.TestConnectionToAPI();

            TestViewModel model = new TestViewModel
            {
                ConnectionAPI = result.ConnectionAPI,
                ConnectionDB = result.ConnectionDB,
                MessageAPI = result.MessageAPI,
                MessageDB = result.MessageDB,
                HTTPResposeAPI = result.HTTPResponseAPI
            };

            return View(model);
        }
    }
}
