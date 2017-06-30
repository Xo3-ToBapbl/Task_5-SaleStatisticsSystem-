﻿using StatisticSystem.DAL.Interfaces;
using System;
using System.Threading.Tasks;
using StatisticSystem.DAL.EF;
using Microsoft.AspNet.Identity.EntityFramework;
using StatisticSystem.DAL.Entities;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Data;

namespace StatisticSystem.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private DataBaseContext _dataBase;

        private ManagerRepository _managers;
        private RoleRepository _roles;
        private SalesRepository _sales;


        public DataBaseContext DataBaseContext
        {
            get
            {
                return _dataBase;
            }
        }

        public ManagerRepository Managers
        {
            get
            {
                return _managers;
            }
        }

        public RoleRepository Roles
        {
            get
            {
                return _roles;
            }
        }

        public SalesRepository Sales
        {
            get
            {
                return _sales;
            }
        }


        public UnitOfWork(string connectionString)
        {
            _dataBase = new DataBaseContext(connectionString);

            _managers = new ManagerRepository(new UserStore<Manager>(_dataBase));
            _roles = new RoleRepository(new RoleStore<Role>(_dataBase));
            _sales = new SalesRepository(_dataBase);
        }


        public bool DeleteManager(string Id)
        {
            Manager manager = Managers.FindById(Id);
            if(manager!=null)
            {
                try
                {
                    Sales.DeleteAllByManagerId(Id);
                    Managers.Delete(manager);
                    SaveChanges();
                    return true;
                }
                catch (DataException)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

            
        public async Task SaveAsync()
        {
            await _dataBase.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _dataBase.SaveChanges();
        }


        #region Dispose
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _managers.Dispose();
                    _roles.Dispose();                   
                    _sales.Dispose();
                }
                this.disposed = true;
            }
        }
        #endregion
    }
}
