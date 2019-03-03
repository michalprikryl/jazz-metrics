using System.Threading.Tasks;
using WebApp.Models.Test;

namespace WebApp.Services.Test
{
    public interface ITestService
    {
        Task<TestViewModel> TestConnectionToAPI();
    }
}
