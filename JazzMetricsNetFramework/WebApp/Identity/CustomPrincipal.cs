using System.Linq;
using System.Security.Principal;

namespace WebApp.Identity
{
    public class CustomPrincipal : CustomSerializeModel, IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }

        public bool IsInRole(string role)
        {
            return Roles.Contains(role);
        }
    }

    public class CustomSerializeModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
    }
}