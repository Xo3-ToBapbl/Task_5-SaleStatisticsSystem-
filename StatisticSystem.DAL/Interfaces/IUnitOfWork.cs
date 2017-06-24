using StatisticSystem.DAL.Entities;
using StatisticSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSystem.DAL.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        ManagerRepository Managers { get; }
        RoleRepository Roles { get; }
        ManagerProfileRepository ManagerProfiles { get; }
        SalesRepository Sales { get; }
        IEnumerable<Sale> GetSalesByManager(string Id);
        IEnumerable<ManagerProfile> GetManagerProfiles();
        KeyValuePair<int, IEnumerable<ManagerProfile>> GetManagerProfilesSpan(int skipNum, int sizeNum);
        KeyValuePair<int, IEnumerable<Sale>> GetSalesSpan(string id, int skipNum, int sizeNum, string filter);

        Task SaveAsync();
    }
}
