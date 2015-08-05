using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_2STBV.Web.Startup))]
namespace _2STBV.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
