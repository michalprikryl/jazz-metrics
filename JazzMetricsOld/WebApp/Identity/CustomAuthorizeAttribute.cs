using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp.Identity
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                //zatim nepotrebuju
                //Roles = string.IsNullOrEmpty(Roles) ? ConfigurationManager.AppSettings[RolesConfigKey] : Roles;

                if (!string.IsNullOrEmpty(Roles))
                {
                    if (!CurrentUser.IsInRole(Roles))
                    {
                        filterContext.Result = new RedirectToRouteResult
                        (
                            new RouteValueDictionary(new { controller = "Error", action = "Lockout" })
                        );
                    }
                }
            }
        }
    }
}