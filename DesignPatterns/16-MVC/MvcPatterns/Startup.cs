using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcPatterns.Startup))]
namespace MvcPatterns
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
