using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Swishlist.Startup))]
namespace Swishlist
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
