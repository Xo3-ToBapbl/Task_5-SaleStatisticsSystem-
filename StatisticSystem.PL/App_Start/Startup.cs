using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Ninject;
using Owin;
using StatisticSystem.BLL.Interfaces;
using System.Configuration;

[assembly: OwinStartup(typeof(StatisticSystem.PL.App_Start.StartUp))]

namespace StatisticSystem.PL.App_Start
{
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IServiceBLL>(CreateServiceBLL);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }

        private IServiceBLL CreateServiceBLL()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ManagersDataBaseConnection"].ConnectionString;
            StandardKernel kernel = new StandardKernel(new NinjectModulePL(connectionString));
            IServiceBLL serviceBLL = kernel.Get<IServiceBLL>();
            return serviceBLL;
        }
    }
}