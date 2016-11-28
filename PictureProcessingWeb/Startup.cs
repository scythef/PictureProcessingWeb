using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PictureProcessingWeb.Startup))]
namespace PictureProcessingWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
