using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DS.UI.PrestadorDeServico.Startup))]
namespace DS.UI.PrestadorDeServico
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
