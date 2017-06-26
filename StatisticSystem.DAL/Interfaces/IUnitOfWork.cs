using StatisticSystem.DAL.Entities;
using StatisticSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StatisticSystem.DAL.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        ManagerRepository Managers { get; }
        RoleRepository Roles { get; }
        SalesRepository Sales { get; }

        IEnumerable<Sale> GetSalesByManager(string Id, string filter, string filterValue);
        Task SaveAsync();
        void SaveChanges();
    }
}
