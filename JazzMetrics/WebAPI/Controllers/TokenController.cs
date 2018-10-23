using WebAPI.Classes.Helpers;
using System.Security.Claims;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class TokenController : ApiController
    {
        public object Post()
        {
            ClaimsPrincipal user = User as ClaimsPrincipal;

            return new { Token = JwtManager.GenerateToken(user.FindFirst(ClaimTypes.Name).Value) };
        }
    }
}
