using System.Security.Claims;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class CustomAPIController : ApiController
    {
        /// <summary>
        /// vrati identitu aktualniho uzivatele, jsou vyplnene name (username) a role (workplace)
        /// </summary>
        public ClaimsIdentity UserIdentity
        {
            get { return User.Identity as ClaimsIdentity; }
        }
    }
}
