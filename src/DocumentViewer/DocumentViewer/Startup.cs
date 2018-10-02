using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DocumentViewer.Startup))]
namespace DocumentViewer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
