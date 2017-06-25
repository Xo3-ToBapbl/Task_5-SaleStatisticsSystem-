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

        public void Update(Sale item)
        {
            DataBase.Entry(item).State = System.Data.Entity.EntityState.Modified;
            DataBase.SaveChanges();
        }

        public void Delete(string id)
        {
            Sale sale = DataBase.Sales.Find(id);
            DataBase.Entry(sale).State = System.Data.Entity.EntityState.Deleted;
            DataBase.SaveChanges();
        }

        public Sale Find(Func<Sale, bool> predicate)
        {
            return DataBase.Sales.FirstOrDefault(predicate);
        }

        public IEnumerable<Sale> GetSalesByManager(string Id, string filter, string filterValue)
        {
            switch(filter)
            {
                case ("Date"):
                    {
                        DateTime outDate;
                        if (DateTime.TryParse(filterValue, out outDate))
                        {
                            return DataBase.Sales.Where(sale => sale.ManagerId == Id && sale.Date == outDate).ToList();
                        }
                        else
                        {
                            return null;
                        }
                    }
                case ("Client"):
                    {
                        return DataBase.Sales.Where(sale => sale.ManagerId == Id && sale.Client == filterValue).ToList();
                    }
                case ("Product"):
                    {
                        return DataBase.Sales.Where(sale => sale.ManagerId == Id && sale.Product == filterValue).ToList();
                    }
                default:
                    {
                        return DataBase.Sales.Where(sale => sale.ManagerId == Id).ToList();
                    }
            }
        }  


        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
