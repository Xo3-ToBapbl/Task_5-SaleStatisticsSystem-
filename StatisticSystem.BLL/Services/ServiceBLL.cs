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


        public async Task<OperationDetails> Create(ManagerDTO managerDTO)
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
                ManagerProfile managerDALProfile = new ManagerProfile { Id = managerDAL.Id, SecondName = managerDTO.UserName };
                DataBase.ManagerProfiles.Create(managerDALProfile);
                await DataBase.SaveAsync();
                return new OperationDetails(true, "Manager add to service.", "");
            }
            else
            {
                return new OperationDetails(false, "Manager with current name already exist.", "UserName");
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


        public KeyValuePair<int, IEnumerable<ManagerProfileDTO>> GetManagersSpan(int skipNum, int sizeNum)
        {
            KeyValuePair<int, IEnumerable<ManagerProfile>> pairDAL = 
                DataBase.ManagerProfiles.GetManagerProfilesSpan(skipNum, sizeNum);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ManagerProfile, ManagerProfileDTO>().ForMember(x => x.Sales, opt => opt.Ignore());
            });
            Mapper.AssertConfigurationIsValid();

            IEnumerable<ManagerProfileDTO> managerPofilesDTO = 
                Mapper.Map<IEnumerable<ManagerProfile>, IEnumerable<ManagerProfileDTO>>(pairDAL.Value);

            KeyValuePair<int, IEnumerable<ManagerProfileDTO>> pairDTO =
                new KeyValuePair<int, IEnumerable<ManagerProfileDTO>>(pairDAL.Key, managerPofilesDTO);
            return pairDTO;
        }

        public KeyValuePair<int, IEnumerable<SaleDTO>> GetSalesSpan(string id, int skipNum, int sizeNum, string filter)
        {
            KeyValuePair<int, IEnumerable<Sale>> pairDAL = DataBase.GetSalesSpan(id, skipNum, sizeNum, filter);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Sale, SaleDTO>();
            });
            Mapper.AssertConfigurationIsValid();

            IEnumerable<SaleDTO> saleDTO = Mapper.Map<IEnumerable<Sale>, IEnumerable<SaleDTO>>(pairDAL.Value);

            KeyValuePair<int, IEnumerable<SaleDTO>> pairDTO = new KeyValuePair<int, IEnumerable<SaleDTO>>(pairDAL.Key, saleDTO);
            return pairDTO;

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

        public IEnumerable<ManagerProfileDTO> GetManagerProfiles()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ManagerProfile, ManagerProfileDTO>().
                    ForMember(dest=>dest.Sales, option=>option.Ignore());                
            });
            Mapper.AssertConfigurationIsValid();

            IEnumerable<ManagerProfile> managersDAL = DataBase.GetManagerProfiles();
            if (managersDAL!=null)
            {
                return Mapper.Map<IEnumerable<ManagerProfile>, IEnumerable<ManagerProfileDTO>>(managersDAL);
            }

            return null;
        }


        public void Dispose()
        {
            DataBase.Dispose();
        }

        
    }
}
