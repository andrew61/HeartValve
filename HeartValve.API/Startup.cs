using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(HeartValve.API.Startup))]

namespace HeartValve.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
