using StatisticSystem.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StatisticSystem.BLL.DTO;
using System.Security.Claims;
using Ninject;
using StatisticSystem.DAL.Interfaces;
using StatisticSystem.DAL.Entities;
using Microsoft.AspNet.Identity;
using AutoMapper;
using System.Linq.Expressions;
using System.Linq;

namespace StatisticSystem.BLL.Services
{
    public class ServiceBLL : IServiceBLL
    {
        private StandardKernel _kernel;

        public IUnitOfWork DataBase { get; set; }


        public ServiceBLL(string connectionString)
        {
            _kernel = new StandardKernel(new NinjectModuleBLL(connectionString));
            DataBase = _kernel.Get<IUnitOfWork>();
        }


        public async Task<OperationDetails> Add(ManagerDTO managerDTO)
        {
            var manager = await DataBase.Managers.FindByNameAsync(managerDTO.UserName);
            if (manager == null)
            {
                var managerDAL = new Manager { UserName = managerDTO.UserName };
                var result = await DataBase.Managers.CreateAsync(managerDAL, managerDTO.Password);
                if (result.Errors.Count() > 0)
                {
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }
                await DataBase.Managers.AddToRoleAsync(managerDAL.Id, managerDTO.Role);
                await DataBase.SaveAsync();
                return new OperationDetails(true, "Manager add to service.", "");
            }
            else
            {
                return new OperationDetails(false, "Manager with current name already exist.", "UserName");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(ManagerDTO adminDTO)
        {
            ClaimsIdentity claim = null;
            Manager manager = await DataBase.Managers.FindAsync(adminDTO.UserName, adminDTO.Password);
            if (manager!=null)
            {
                claim = await DataBase.Managers.CreateIdentityAsync(manager, DefaultAuthenticationTypes.ApplicationCookie);
            }
            return claim;
        }

        public async Task SetInitialData(ManagerDTO userDTO, List<string> roles)
        {
            foreach(string roleName in roles)
            {
                var role = await DataBase.Roles.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new Role { Name = roleName };
                    await DataBase.Roles.CreateAsync(role);
                }
            }
            await Add(userDTO);
        }

        public async Task<ManagerDTO> GetManagerById(string id)
        {
            Manager managerDAL = await DataBase.Managers.FindByIdAsync(id);
            var role = await DataBase.Managers.GetRolesAsync(id);
            if (managerDAL!=null)
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Manager, ManagerDTO>();
                });

                ManagerDTO managerDTO = Mapper.Map<Manager, ManagerDTO>(managerDAL);
                managerDTO.Role = role.FirstOrDefault();
                return managerDTO;
            }
            return null;
        }

        public IEnumerable<SaleDTO> GetSalesByManager(string id)
        {
            IEnumerable<Sale> salesDAL = DataBase.GetSalesByManager(id);
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Sale, SaleDTO>();
            });
            Mapper.AssertConfigurationIsValid();
            return Mapper.Map<IEnumerable<Sale>, IEnumerable<SaleDTO>>(salesDAL);

        }


        public void Dispose()
        {
            DataBase.Dispose();
        }

        
    }
}
