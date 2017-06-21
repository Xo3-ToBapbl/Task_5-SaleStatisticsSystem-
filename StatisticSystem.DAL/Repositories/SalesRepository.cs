using StatisticSystem.DAL.EF;
using StatisticSystem.DAL.Entities;
using StatisticSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSystem.DAL.Repositories
{
    public class SalesRepository: IRepository<Sale>, IDisposable
    {
        public DataBaseContext DataBase { get; set; }

        public int Count
        {
            get
            {
                return DataBase.Sales.Count();
            }
        }

        public SalesRepository(DataBaseContext dataBase)
        {
            DataBase = dataBase;
        }

        public void Create(Sale item)
        {
            DataBase.Sales.Add(item);
        }

        public Sale Find(Func<Sale, bool> predicate)
        {
            return DataBase.Sales.FirstOrDefault(predicate);
        }

        public IEnumerable<Sale> GetSalessById(string Id)
        {
            return DataBase.Sales.Where(x => x.ManagerProfileId == Id).ToList();
        }


        public void Dispose()
        {
            DataBase.Dispose();
        }

        public IEnumerable<Sale> GetSpan(int skipNum, int sizeNum)
        {
            return DataBase.Sales.OrderBy(x=>x.Client).Skip(skipNum).Take(sizeNum).ToList();
        }
    }
}
