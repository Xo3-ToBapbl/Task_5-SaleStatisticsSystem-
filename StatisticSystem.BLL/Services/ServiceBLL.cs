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
using StatisticSystem.DAL.Entities;
using Microsoft.AspNet.Identity;
using AutoMapper;

namespace StatisticSystem.BLL.Services
{
    public class ServiceBLL : IServiceBLL
    {
        private StandardKernel _kernel;

        public IUnitOfWork DataBase { get; set; }

        public int ManagersCount
        {
            get
            {
                return DataBase.ManagerProfiles.Count;
            }
        }


        public ServiceBLL(string connectionString)
        {
            _kernel = new StandardKernel(new NinjectModuleBLL(connectionString));
            DataBase = _kernel.Get<IUnitOfWork>();
        }


        public async Task Create(ManagerDTO managerDTO)
        {
            var manager = await DataBase.Managers.FindByNameAsync(managerDTO.UserName);
            if (manager == null)
            {
                var managerDAL = new Manager { UserName = managerDTO.UserName };
                await DataBase.Managers.CreateAsync(managerDAL, managerDTO.Password);
                await DataBase.Managers.AddToRoleAsync(managerDAL.Id, managerDTO.Role);
                ManagerProfile managerDALProfile = new ManagerProfile { Id = managerDAL.Id, SecondName = managerDTO.UserName };
                DataBase.ManagerProfiles.Create(managerDALProfile);
                await DataBase.SaveAsync();
            }
        }

        public async Task<ClaimsIdentity> Authenticate(ManagerDTO userDTO)
        {
            ClaimsIdentity claim = null;
            Manager manager = await DataBase.Managers.FindAsync(userDTO.UserName, userDTO.Password);
            if (manager!=null)
            {
                claim = await DataBase.Managers.CreateIdentityAsync(manager, DefaultAuthenticationTypes.ApplicationCookie);
            }
            return claim;
        }

        public Task SetInitialData(ManagerDTO adminDTO, List<string> roles)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ManagerProfileDTO> GetSpanManagers(int skipNum, int sizeNum)
        {
            IEnumerable<ManagerProfile> managersDAL = DataBase.ManagerProfiles.GetSpan(skipNum, sizeNum);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ManagerProfile, ManagerProfileDTO>().ForMember(x => x.Sales, opt => opt.Ignore());
            });
            Mapper.AssertConfigurationIsValid();

            IEnumerable<ManagerProfileDTO> managersDTO = 
                Mapper.Map<IEnumerable<ManagerProfile>, IEnumerable<ManagerProfileDTO>>(managersDAL);

            return managersDTO;
        }

        public IEnumerable<SaleDTO> GetSalesById(string Id)
        {
            IEnumerable<Sale> salesDAL = DataBase.GetSalesById(Id);
            if (salesDAL.Count()!=0)
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Sale, SaleDTO>();
                });
                Mapper.AssertConfigurationIsValid();

                return Mapper.Map<IEnumerable<Sale>, IEnumerable<SaleDTO>>(salesDAL);
            }
            else
            {
                return null;
            }
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }

        
    }
}
