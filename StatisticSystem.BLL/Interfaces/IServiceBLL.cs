using StatisticSystem.BLL.DTO;
using StatisticSystem.BLL.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StatisticSystem.BLL.Interfaces
{
    public interface IServiceBLL:IDisposable
    {
        Task<OperationDetails> AddManager(ManagerDTO managerDTO);
        Task<ClaimsIdentity> Authenticate(ManagerDTO userDTO);
        ManagerDTO GetManagerById(string id);
        IEnumerable<SaleDTO> GetSalesByManager(string id, string filter, string filterValue);
        IEnumerable<ManagerDTO> GetManagers();
        Task<OperationDetails> UpdateSale(SaleDTO saleDTO);
        Task<OperationDetails> DeleteSale(string id);
        Dictionary<DateTime, int> GetDateSalesCount(string managerId);
        Dictionary<SaleDTO, string> GetFiltredSales(string filter, string filterValue);
        Task<OperationDetails> DeleteManager(string Id);
        SaleDTO GetSale(string id);
    }
}
