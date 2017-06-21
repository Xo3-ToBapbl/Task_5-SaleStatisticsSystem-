using StatisticSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSystem.DAL.UtilClasses
{
    public class SalesInfo
    {
        public int Count { get; set; }
        public ICollection<Sale> Sales { get; set; }
    }
}
