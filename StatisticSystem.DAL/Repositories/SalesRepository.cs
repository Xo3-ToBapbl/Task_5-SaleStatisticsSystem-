using StatisticSystem.DAL.EF;
using StatisticSystem.DAL.Entities;
using StatisticSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSystem.DAL.Repositories
{
    public class SalesRepository: IDisposable
    {
        public SalesRepository(DataBaseContext dataBase)
        {
            DataBase = dataBase;
        }


        public DataBaseContext DataBase { get; set; }


        public void Add(Sale item)
        {
            DataBase.Sales.Add(item);
        }

        public Sale Find(Func<Sale, bool> predicate)
        {
            return DataBase.Sales.FirstOrDefault(predicate);
        }

        public IEnumerable<Sale> GetSalesByManager(string Id)
        {
            return DataBase.Sales.Where(x => x.ManagerId == Id).ToList();
        }  


        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
