using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlogWebApp.Startup))]
namespace BlogWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
