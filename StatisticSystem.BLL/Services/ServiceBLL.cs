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
using System.Linq;
using System.Data;

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

        public IEnumerable<ManagerDTO> GetManagers()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Manager, ManagerDTO>().ForMember(dest=>dest.Sales, opt=>opt.Ignore());
            });
            IEnumerable<Manager> managerDAL = DataBase.Managers.Users.ToList();
            return Mapper.Map<IEnumerable <Manager> ,IEnumerable<ManagerDTO>>(managerDAL);
        }

        public IEnumerable<SaleDTO> GetSalesByManager(string id, string filter, string filterValue)
        {
            IEnumerable<Sale> salesDAL = DataBase.GetSalesByManager(id, filter, filterValue);
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Sale, SaleDTO>();
            });
            Mapper.AssertConfigurationIsValid();
            return Mapper.Map<IEnumerable<Sale>, IEnumerable<SaleDTO>>(salesDAL);

        }

        public OperationDetails UpdateSale(SaleDTO saleDTO)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SaleDTO, Sale>();
            });

            OperationDetails details;
            try
            {
                DataBase.Sales.Update(Mapper.Map<SaleDTO, Sale>(saleDTO));
                details = new OperationDetails(true, "Sale update in data base.", "");
            }
            catch (DataException)
            {
                details = new OperationDetails(true, "Unable to update sale. Inner error.", "");
            }
            return details;
        }

        public OperationDetails DeleteSale(string id)
        {
            OperationDetails details;
            try
            {
                DataBase.Sales.Delete(id);
                details = new OperationDetails(true, "Sale delete from data base.", "");
            }
            catch (DataException)
            {
                details = new OperationDetails(true, "Unable to delete sale. Inner error.", "");
            }
            return details;
        }

        public Dictionary<DateTime, int> GetDateSalesCount(string managerId)
        {
            return DataBase.Sales.GetDateSalesCount(managerId);
        }

        public Dictionary<SaleDTO, string> GetFiltredSales(string filter, string filterValue)
        {
            Dictionary<Sale, string> filtredSalesDAL = DataBase.Sales.GetFiltredSales(filter, filterValue);
            Dictionary<SaleDTO, string> filtredSalesDTO = new Dictionary<SaleDTO, string>();
            if (filtredSalesDAL!=null)
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Sale, SaleDTO>();
                });
                filtredSalesDTO = Mapper.Map<Dictionary<Sale, string>, Dictionary<SaleDTO, string>>(filtredSalesDAL);
                return filtredSalesDTO;
            }
            else
            {
                return filtredSalesDTO;
            }

        }


        public void Dispose()
        {
            DataBase.Dispose();
        }

        
    }
}
