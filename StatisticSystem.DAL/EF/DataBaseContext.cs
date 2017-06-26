using Microsoft.AspNet.Identity.EntityFramework;
using StatisticSystem.DAL.Entities;
using System.Data.Entity;

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
