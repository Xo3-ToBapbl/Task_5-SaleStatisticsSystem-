using StatisticSystem.DAL.Interfaces;
using StatisticSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatisticSystem.DAL.Entities;
using StatisticSystem.DAL.EF;
using System.Linq.Expressions;

namespace StatisticSystem.DAL.Repositories
{
    public class ManagerProfileRepository : IDisposable
    {
        public ManagerProfileRepository(DataBaseContext dataBase)
        {
            DataBase = dataBase;
        }


        public DataBaseContext DataBase { get; set; }

        public int Count
        {
            get
            {
                return DataBase.ManagerProfiles.Count();
            }
        }


        public void Create(ManagerProfile item)
        {
            DataBase.ManagerProfiles.Add(item);
        }

        public ManagerProfile Find(Func<ManagerProfile, bool> predicate)
        {
            return DataBase.ManagerProfiles.FirstOrDefault(predicate);
        }

        public IEnumerable<ManagerProfile> GetAll()
        {
            return DataBase.ManagerProfiles.ToList();
        }

        public KeyValuePair<int, IEnumerable<ManagerProfile>> GetManagerProfilesSpan(int skipNum, int sizeNum)
        {
            return new KeyValuePair<int, IEnumerable<ManagerProfile>>
                (DataBase.ManagerProfiles.Count(),
                DataBase.ManagerProfiles.OrderBy(x => x.SecondName).Skip(skipNum).Take(sizeNum).ToList());
        }


        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
