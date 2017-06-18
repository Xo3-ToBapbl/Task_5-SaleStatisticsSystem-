using StatisticSystem.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatisticSystem.BLL.DTO;
using System.Security.Claims;
using Ninject;
using StatisticSystem.DAL.Interfaces;

namespace StatisticSystem.BLL.Services
{
    public class UserService //IUserService
    {
        private StandardKernel _kernel;

        public IUnitOfWork DataBase { get; set; }


        public UserService(string connectionString)
        {
            _kernel = new StandardKernel(new NinjectModuleBLL(connectionString));
            DataBase = _kernel.Get<IUnitOfWork>();
        }


        public Task<ClaimsIdentity> Authenticate(UserDTO userDTO)
        {
            throw new NotImplementedException();
        }

        public Task SetInitialData(UserDTO adminDTO, List<string> roles)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        //public Task Create(UserDTO userDTO)
        //{
            
        //}
    }
}
