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
        SalesRepository Sales { get; }

        IEnumerable<Sale> GetSalesByManager(string Id);
        void UpdateSale(Sale sale);
        Task SaveAsync();
        void SaveChanges();
    }
}
