using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Flierefluiter.Reception.Startup))]
namespace Flierefluiter.Reception
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
