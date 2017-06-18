using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StatisticSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSystem.DAL.Repositories
{
    public class RoleRepository : RoleManager<Role>
    {
        public RoleRepository(RoleStore<Role> store) : base(store)
        {
        }
    }
}
