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


        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
