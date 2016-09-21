using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HighlightCode.Startup))]
namespace HighlightCode
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
