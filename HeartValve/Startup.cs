using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HeartValve.Startup))]
namespace HeartValve
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
