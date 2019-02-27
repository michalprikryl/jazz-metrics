using Microsoft.Owin;
using Owin;

//nutne pro spravne nacitani prav uzivatele (prihlaseny/neprihlaseny aj.)
[assembly: OwinStartup(typeof(WebApp.Startup))]
namespace WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
