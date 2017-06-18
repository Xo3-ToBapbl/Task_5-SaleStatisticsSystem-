using Microsoft.AspNet.Identity;
using StatisticSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSystem.DAL.Repositories
{
    public class ManagerRepository : UserManager<Manager>
    {
        public ManagerRepository(IUserStore<Manager> store) : base(store)
        {
            PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 4,
                RequireDigit = false,
                RequireUppercase = false
            };
        }
    }
}
