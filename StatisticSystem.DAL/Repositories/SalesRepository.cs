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

        public KeyValuePair<int, IEnumerable<Sale>> GetSalesSpan
            (string id, int skipNum, int sizeNum, string filter)
        {
            switch(filter)
            {                
                case ("cost"):
                    {
                        return new KeyValuePair<int, IEnumerable<Sale>>
                (DataBase.Sales.Count(),
                 DataBase.Sales.OrderBy(sale=>sale.Cost).Skip(skipNum).Take(sizeNum).ToList());
                    };
                case ("product"):
                    {
                        return new KeyValuePair<int, IEnumerable<Sale>>
                (DataBase.Sales.Count(),
                 DataBase.Sales.OrderBy(sale => sale.Product).Skip(skipNum).Take(sizeNum).ToList());
                    };
                case ("client"):
                    {
                        return new KeyValuePair<int, IEnumerable<Sale>>
                (DataBase.Sales.Count(),
                 DataBase.Sales.OrderBy(sale => sale.Client).Skip(skipNum).Take(sizeNum).ToList());
                    };
                default:
                    {
                        return new KeyValuePair<int, IEnumerable<Sale>>
                (DataBase.Sales.Count(),
                 DataBase.Sales.OrderBy(sale => sale.Date).Skip(skipNum).Take(sizeNum).ToList());
                    };
            }
            
        }

        public IEnumerable<Sale> GetAll(Expression<Func<Sale, string>> expression)
        {
            return DataBase.Sales.OrderBy(expression).ToList();
        }


        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
