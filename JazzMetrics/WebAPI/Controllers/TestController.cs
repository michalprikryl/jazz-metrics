using WebAPI.Classes.Test;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class TestController : CustomAPIController
    {
        public async Task<object> Get()
        {
            TestMethods methods = new TestMethods();

            return await methods.RunTest();
        }
    }
}