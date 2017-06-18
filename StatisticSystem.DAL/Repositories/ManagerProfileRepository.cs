using StatisticSystem.DAL.Interfaces;
using StatisticSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatisticSystem.DAL.Entities;
using StatisticSystem.DAL.EF;

namespace StatisticSystem.DAL.Repositories
{
    public class ManagerProfileRepository : IRepository<ManagerProfile>, IDisposable
    {
        public DataBaseContext DataBase { get; set; }

        public ManagerProfileRepository(DataBaseContext dataBase)
        {
            DataBase = dataBase;
        }

        public void Create(ManagerProfile item)
        {
            DataBase.ManagerProfiles.Add(item);
        }

        public ManagerProfile Find(Func<ManagerProfile, bool> predicate)
        {
            return DataBase.ManagerProfiles.FirstOrDefault(predicate);
        }


        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
