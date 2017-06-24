using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
