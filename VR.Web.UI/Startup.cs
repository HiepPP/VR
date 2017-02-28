using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VR.Web.UI.Startup))]
namespace VR.Web.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
