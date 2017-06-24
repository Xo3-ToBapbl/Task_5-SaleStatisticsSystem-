using System;

namespace StatisticSystem.BLL.DTO
{
    public class SaleDTO
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }
        public string Client { get; set; }
        public string Product { get; set; }
        public int Cost { get; set; }

        public string ManagerId { get; set; }
    }
}