using System;

namespace StatisticSystem.DAL.Entities
{
    public class Sale
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }
        public string Client { get; set; }
        public string Product { get; set; }
        public int Cost { get; set; }

        public string ManagerId { get; set; }

        public Manager Manager { get; set; }
    }
}
