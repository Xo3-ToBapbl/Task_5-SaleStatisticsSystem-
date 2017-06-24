using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StatisticSystem.DAL.Entities;
using StatisticSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSystem.DAL.EF
{
    public class DataBaseInitializer: CreateDatabaseIfNotExists<DataBaseContext>
    {
        protected override void Seed(DataBaseContext context)
        {
            #region Initial
            ManagerRepository managerRepository = new ManagerRepository(new UserStore<Manager>(context));
            RoleRepository roleRepository = new RoleRepository(new RoleStore<Role>(context));
            Role roleAdmin = new Role { Name = "admin" };
            Role roleUser = new Role { Name = "user" };
            roleRepository.Create(roleAdmin);
            roleRepository.Create(roleUser);
            #endregion

            #region Admin
            Manager admin = new Manager { UserName = "Admin" };
            var result = managerRepository.Create(admin, "1111");
            managerRepository.AddToRole(admin.Id, roleAdmin.Name);
            #endregion

            #region Managers
            List<Sale> sales1 = new List<Sale>
            {
                new Sale {Id=Guid.NewGuid().ToString(), Client="Vasily Utkin",Date=DateTime.Parse("25.06.2017"),Product="Iphone 6",Cost=330 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Genadij Petrov",Date=DateTime.Parse("25.06.2017"),Product="Iphone 6+",Cost=430 },
            };
            Manager manager1 = new Manager { UserName = "Kulagin", Sales = sales1 };
            var result1 = managerRepository.Create(manager1, "2222");
            managerRepository.AddToRole(manager1.Id, roleUser.Name);

            List<Sale> sales2 = new List<Sale>
            {
                new Sale {Id=Guid.NewGuid().ToString(), Client="Alla Pugacheva",Date=DateTime.Parse("25.06.2017"),Product="Cperia Z5",Cost=550 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Andrew Boget",Date=DateTime.Parse("26.06.2017"),Product="Galaxy S6",Cost=880 },
            };
            Manager manager2 = new Manager { UserName = "Kotov", Sales = sales2 };
            var result2 = managerRepository.Create(manager2, "3333");
            managerRepository.AddToRole(manager2.Id, roleUser.Name);
            #endregion

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
