using StatisticSystem.DAL.EF;
using StatisticSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public Dictionary<DateTime, int> GetDateSalesCount(string ManagerId)
        {
            Dictionary<DateTime, int> result = (from sale in DataBase.Sales
                       where sale.ManagerId==ManagerId
                       group sale by sale.Date).ToDictionary(x => x.Key, x => x.Count());

            return result;
        }


        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
