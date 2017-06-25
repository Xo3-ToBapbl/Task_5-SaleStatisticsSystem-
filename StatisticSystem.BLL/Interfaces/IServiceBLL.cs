using StatisticSystem.BLL.DTO;
using StatisticSystem.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSystem.BLL.Interfaces
{
    public interface IServiceBLL:IDisposable
    {
        Task<OperationDetails> Add(ManagerDTO userDTO);
        Task<ClaimsIdentity> Authenticate(ManagerDTO userDTO);
        Task<ManagerDTO> GetManagerById(string id);
        IEnumerable<SaleDTO> GetSalesByManager(string id);
        IEnumerable<ManagerDTO> GetManagers();
        OperationDetails UpdateSale(SaleDTO saleDTO);
    }
}
