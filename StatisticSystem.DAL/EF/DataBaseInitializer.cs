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
            Manager admin = new Manager { UserName = "Warden" };
            var result = managerRepository.Create(admin, "1111");
            managerRepository.AddToRole(admin.Id, roleAdmin.Name);
            #endregion

            #region Managers
            List<Sale> sales1 = new List<Sale>
            {
                new Sale {Id=Guid.NewGuid().ToString(), Client="Vasily Utkin",Date=DateTime.Parse("25.06.2017"),Product="Iphone 6",Cost=330 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Andrew Petrov",Date=DateTime.Parse("25.06.2017"),Product="Galaxy S6",Cost=880 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Genadij Petrov",Date=DateTime.Parse("25.06.2017"),Product="Iphone 6+",Cost=430 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Phill Glory",Date=DateTime.Parse("26.06.2017"),Product="Ziljan K2",Cost=220 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Igor Ulej",Date=DateTime.Parse("26.06.2017"),Product="Xperia Z5",Cost=550 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Igor Ulej",Date=DateTime.Parse("26.06.2017"),Product="Galaxy S6",Cost=880 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="James Jonsin",Date=DateTime.Parse("26.06.2017"),Product="GameBoy",Cost=550 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Piter Parker",Date=DateTime.Parse("27.06.2017"),Product="SegaMegaDrive",Cost=350 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Piter Parker",Date=DateTime.Parse("27.06.2017"),Product="Huawei",Cost=150 },
            };
            Manager manager1 = new Manager { UserName = "Kulagin", Sales = sales1 };
            var result1 = managerRepository.Create(manager1, "2222");
            managerRepository.AddToRole(manager1.Id, roleUser.Name);
            //------------------------------------------------------
            List<Sale> sales2 = new List<Sale>
            {
                new Sale {Id=Guid.NewGuid().ToString(), Client="Alla Pugacheva",Date=DateTime.Parse("25.06.2017"),Product="Xperia Z5",Cost=550 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Andrew Boget",Date=DateTime.Parse("26.06.2017"),Product="Galaxy S6",Cost=880 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Jon Snow",Date=DateTime.Parse("26.06.2017"),Product="GameBoy",Cost=550 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Lory Row",Date=DateTime.Parse("27.06.2017"),Product="Huawei",Cost=150 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Tori Gilfojd",Date=DateTime.Parse("27.06.2017"),Product="Xperia Z5",Cost=550},
            };
            Manager manager2 = new Manager { UserName = "Kotov", Sales = sales2 };
            var result2 = managerRepository.Create(manager2, "3333");
            managerRepository.AddToRole(manager2.Id, roleUser.Name);
            //------------------------------------------------------
            List<Sale> sales3 = new List<Sale>
            {
                new Sale {Id=Guid.NewGuid().ToString(), Client="Lena Patrova",Date=DateTime.Parse("25.06.2017"),Product="Redmi X6",Cost=660 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Lena Petrova",Date=DateTime.Parse("25.06.2017"),Product="Galaxy S6",Cost=880 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Pit Pitterson",Date=DateTime.Parse("26.06.2017"),Product="LG G2",Cost=660 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Hujy Lory",Date=DateTime.Parse("27.06.2017"),Product="IPad",Cost=1150 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Mat Stivens",Date=DateTime.Parse("27.06.2017"),Product="IPod",Cost=350},
                new Sale {Id=Guid.NewGuid().ToString(), Client="Mat Stivens",Date=DateTime.Parse("27.06.2017"),Product="IPod",Cost=350},
                new Sale {Id=Guid.NewGuid().ToString(), Client="Mat Stivens",Date=DateTime.Parse("27.06.2017"),Product="IPhone 6",Cost=330},
                new Sale {Id=Guid.NewGuid().ToString(), Client="Bill Burray",Date=DateTime.Parse("28.06.2017"),Product="SegaMegaDrive",Cost=350},
            };
            Manager manager3 = new Manager { UserName = "Mustyac", Sales = sales3 };
            var result3 = managerRepository.Create(manager3, "4444");
            managerRepository.AddToRole(manager3.Id, roleUser.Name);
            //------------------------------------------------------
            Manager manager4 = new Manager { UserName = "Ivanov" };
            var result4 = managerRepository.Create(manager4, "5555");
            managerRepository.AddToRole(manager4.Id, roleUser.Name);
            //------------------------------------------------------
            Manager manager5 = new Manager { UserName = "Sidorov" };
            var result5 = managerRepository.Create(manager5, "6666");
            managerRepository.AddToRole(manager5.Id, roleUser.Name);
            //------------------------------------------------------
            Manager manager6 = new Manager { UserName = "Kolesnikov" };
            var result6 = managerRepository.Create(manager6, "7777");
            managerRepository.AddToRole(manager6.Id, roleUser.Name);
            //------------------------------------------------------
            Manager manager7 = new Manager { UserName = "Viskub" };
            var result7 = managerRepository.Create(manager7, "8888");
            managerRepository.AddToRole(manager7.Id, roleUser.Name);
            //------------------------------------------------------
            Manager manager8 = new Manager { UserName = "Stolen" };
            var result8 = managerRepository.Create(manager8, "9999");
            managerRepository.AddToRole(manager8.Id, roleUser.Name);
            #endregion

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
