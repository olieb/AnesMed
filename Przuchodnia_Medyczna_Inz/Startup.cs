using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Przuchodnia_Medyczna_Inz.Startup))]
namespace Przuchodnia_Medyczna_Inz
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
