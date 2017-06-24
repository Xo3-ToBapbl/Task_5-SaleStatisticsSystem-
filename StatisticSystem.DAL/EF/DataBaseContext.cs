using Microsoft.AspNet.Identity.EntityFramework;
using StatisticSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSystem.DAL.EF
{
    public class DataBaseContext: IdentityDbContext<Manager>
    {
        public DataBaseContext(string connectionString):base(connectionString)
        {
            Database.SetInitializer<DataBaseContext>(new DataBaseInitializer());
        }

        public DbSet<Sale> Sales { get; set; }
    }
}
