﻿using StatisticSystem.DAL.Entities;
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
        IRepository<ManagerProfile> ManagerProfiles { get; }
        IRepository<Sale> Sales { get; }
        IEnumerable<Sale> GetSalesById(string Id);
        IEnumerable<ManagerProfile> GetManagerProfiles(Expression<Func<ManagerProfile, string>> expression);

        Task SaveAsync();
    }
}
