using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SaleStatisticsSystemPL.Startup))]
namespace SaleStatisticsSystemPL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
