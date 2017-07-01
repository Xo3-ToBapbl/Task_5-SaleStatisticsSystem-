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

        public Sale GetSale(string id)
        {
            return DataBase.Sales.FirstOrDefault(sale => sale.Id == id);
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

        public void DeleteAllByManagerId(string managerId)
        {
            var salesId = DataBase.Sales.Where(sale => sale.ManagerId == managerId).ToList();
            salesId.ForEach(sale => DataBase.Entry(sale).State = System.Data.Entity.EntityState.Deleted);
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

        public Dictionary<Sale, string> GetFiltredSales(string filter, string filterValue=null)
        {
            switch(filter)
            {
                case ("Date"):
                    {
                        DateTime date;
                        if (DateTime.TryParse(filterValue, out date))
                        {
                            return (from sale in DataBase.Sales
                                          where sale.Date == date
                                          join manager in DataBase.Users on sale.ManagerId equals manager.Id
                                          select new { Sale = sale, ManagerSecondName = manager.UserName }).
                                     ToDictionary(x => x.Sale, x => x.ManagerSecondName);
                        }
                        else
                        {
                            return null;
                        }
                        
                    }
                case ("Client"):
                    {
                        return (from sale in DataBase.Sales
                                where sale.Client == filterValue
                                join manager in DataBase.Users on sale.ManagerId equals manager.Id
                                select new { Sale = sale, ManagerSecondName = manager.UserName }).
                                     ToDictionary(x => x.Sale, x => x.ManagerSecondName);
                    }
                case ("Product"):
                    {
                        return (from sale in DataBase.Sales
                                where sale.Product == filterValue
                                join manager in DataBase.Users on sale.ManagerId equals manager.Id
                                select new { Sale = sale, ManagerSecondName = manager.UserName }).
                                     ToDictionary(x => x.Sale, x => x.ManagerSecondName);
                    }
                default:
                    return null;
            }
            
        }


        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
