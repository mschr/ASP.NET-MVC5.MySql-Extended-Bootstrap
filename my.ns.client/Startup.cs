using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(my.ns.client.Startup))]
namespace my.ns.client
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureLogger();
            ConfigureAuth(app);
        }
    }
}
