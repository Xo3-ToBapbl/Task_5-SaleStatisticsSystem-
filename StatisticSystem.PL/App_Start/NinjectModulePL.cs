using Ninject.Modules;
using StatisticSystem.BLL.Interfaces;
using StatisticSystem.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatisticSystem.PL.App_Start
{
    public class NinjectModulePL: NinjectModule
    {
        private string _connectionString;

        public NinjectModulePL(string connectionString)
        {
            _connectionString = connectionString;
        }


        public override void Load()
        {
            Bind<IServiceBLL>().To<ServiceBLL>().WithConstructorArgument("connectionString", _connectionString);
        }
    }
}