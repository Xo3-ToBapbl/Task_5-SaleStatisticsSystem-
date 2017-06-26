using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StatisticSystem.DAL.Entities;

namespace StatisticSystem.DAL.Repositories
{
    public class RoleRepository : RoleManager<Role>
    {
        public RoleRepository(RoleStore<Role> store) : base(store)
        {
        }
    }
}
