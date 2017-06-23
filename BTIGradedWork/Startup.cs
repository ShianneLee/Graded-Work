using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BTIGradedWork.Startup))]
namespace BTIGradedWork
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
