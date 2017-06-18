using Ninject.Modules;
using StatisticSystem.DAL.Interfaces;
using StatisticSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSystem.BLL.Services
{
    public class NinjectModuleBLL : NinjectModule
    {
        private string _connectionString;

        public NinjectModuleBLL(string connectionString)
        {
            _connectionString = connectionString;
        }


        public override void Load()
        {
            //Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument("connectionString", _connectionString);
        }
    }
}
