using StatisticSystem.BLL.DTO;
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
        Task Create(ManagerDTO userDTO);
        Task<ClaimsIdentity> Authenticate(ManagerDTO userDTO);
        Task SetInitialData(ManagerDTO adminDTO, List<string> roles);

        KeyValuePair<int, IEnumerable<ManagerProfileDTO>> GetManagersSpan(int skipNum, int sizeNum);
        KeyValuePair<int, IEnumerable<SaleDTO>> GetSalesSpan(string id, int skipNum, int sizeNum, string filter);
        IEnumerable<ManagerProfileDTO> GetManagers(Expression<Func<ManagerProfileDTO, string>> expression);
    }
}
