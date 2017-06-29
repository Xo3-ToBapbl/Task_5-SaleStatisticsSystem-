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
        Task<OperationDetails> Add(ManagerDTO userDTO);
        Task<ClaimsIdentity> Authenticate(ManagerDTO userDTO);
        Task<ManagerDTO> GetManagerById(string id);
        IEnumerable<SaleDTO> GetSalesByManager(string id, string filter, string filterValue);
        IEnumerable<ManagerDTO> GetManagers();
        OperationDetails UpdateSale(SaleDTO saleDTO);
        OperationDetails DeleteSale(string id);
        Dictionary<DateTime, int> GetDateSalesCount(string managerId);
        Dictionary<SaleDTO, string> GetFiltredSales(string filter, string filterValue);
        KeyValuePair<string, List<string>> GetManager(string Id);
    }
}
