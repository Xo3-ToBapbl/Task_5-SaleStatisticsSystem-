using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StatisticSystem.DAL.Entities;
using StatisticSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;

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
            List<Sale> sales = new List<Sale>
            {
                new Sale {Id=Guid.NewGuid().ToString(), Client="Anton Pankov",Date=DateTime.Parse("25.06.2017"),Product="Aser VZ1",Cost=690 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Andrew Petrov",Date=DateTime.Parse("25.06.2017"),Product="Galaxy Note",Cost=760 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Bill Gates",Date=DateTime.Parse("26.06.2017"),Product="Samsun TV5G",Cost=540 },           
            };
            Manager admin = new Manager { UserName = "Warden", Sales=sales };
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
                new Sale {Id=Guid.NewGuid().ToString(), Client="Piter Parker",Date=DateTime.Parse("27.06.2017"),Product="Meize",Cost=550 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Phill Glory",Date=DateTime.Parse("28.06.2017"),Product="HTC One",Cost=1150 },
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
                new Sale {Id=Guid.NewGuid().ToString(), Client="Tori Gilfojd",Date=DateTime.Parse("27.06.2017"),Product="Xperia XZ",Cost=920},
                new Sale {Id=Guid.NewGuid().ToString(), Client="Toni Mager",Date=DateTime.Parse("28.06.2017"),Product="Nokia 3310",Cost=20},
            };
            Manager manager2 = new Manager { UserName = "Kotov", Sales = sales2 };
            var result2 = managerRepository.Create(manager2, "3333");
            managerRepository.AddToRole(manager2.Id, roleUser.Name);
            //------------------------------------------------------
            List<Sale> sales3 = new List<Sale>
            {
                new Sale {Id=Guid.NewGuid().ToString(), Client="Lena Pashkova",Date=DateTime.Parse("25.06.2017"),Product="Redmi X6",Cost=660 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Lena Pashkova",Date=DateTime.Parse("25.06.2017"),Product="Galaxy S6",Cost=880 },
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
            List<Sale> sales4 = new List<Sale>
            {
                new Sale {Id=Guid.NewGuid().ToString(), Client="Lena Patrova",Date=DateTime.Parse("25.06.2017"),Product="Redmi X6",Cost=660 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Lena Petrova",Date=DateTime.Parse("25.06.2017"),Product="Galaxy S6",Cost=880 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Vasily Utkin",Date=DateTime.Parse("26.06.2017"),Product="LG G2",Cost=660 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Hujy Lory",Date=DateTime.Parse("28.06.2017"),Product="IPad",Cost=1150 },              
            };
            Manager manager4 = new Manager { UserName = "Ivanov", Sales = sales4 };
            var result4 = managerRepository.Create(manager4, "5555");
            managerRepository.AddToRole(manager4.Id, roleUser.Name);
            //------------------------------------------------------
            List<Sale> sales5 = new List<Sale>
            {
                new Sale {Id=Guid.NewGuid().ToString(), Client="ALena Danilova",Date=DateTime.Parse("24.06.2017"),Product="HP P8",Cost=460 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Avdej Petrov",Date=DateTime.Parse("25.06.2017"),Product="Galaxy S4",Cost=550 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Avdej Petrov",Date=DateTime.Parse("25.06.2017"),Product="LG G2",Cost=660 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Jan Piters",Date=DateTime.Parse("26.06.2017"),Product="IPod",Cost=200 },
            };
            Manager manager5 = new Manager { UserName = "Sidorov", Sales = sales5 };
            var result5 = managerRepository.Create(manager5, "6666");
            managerRepository.AddToRole(manager5.Id, roleUser.Name);
            //------------------------------------------------------
            List<Sale> sales6 = new List<Sale>
            {
                new Sale {Id=Guid.NewGuid().ToString(), Client="Bridjet Jonson",Date=DateTime.Parse("25.06.2017"),Product="Huawei P8",Cost=560 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Avdej Petrov",Date=DateTime.Parse("25.06.2017"),Product="Galaxy S4",Cost=550 },
            };
            Manager manager6 = new Manager { UserName = "Kolesnikov", Sales=sales6 };
            var result6 = managerRepository.Create(manager6, "7777");
            managerRepository.AddToRole(manager6.Id, roleUser.Name);
            //------------------------------------------------------
            List<Sale> sales7 = new List<Sale>
            {
                new Sale {Id=Guid.NewGuid().ToString(), Client="Adelina Home",Date=DateTime.Parse("25.06.2017"),Product="Pixels V2",Cost=360 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Lena Pashkova",Date=DateTime.Parse("25.06.2017"),Product="Nokia Lumia",Cost=680 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Pit Pitterson",Date=DateTime.Parse("25.06.2017"),Product="LG G2",Cost=560 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Jason Statham",Date=DateTime.Parse("27.06.2017"),Product="IPad",Cost=950 },
                new Sale {Id=Guid.NewGuid().ToString(), Client="Batman Robin",Date=DateTime.Parse("27.06.2017"),Product="LG G4",Cost=450},
                new Sale {Id=Guid.NewGuid().ToString(), Client="Ivan Urgant",Date=DateTime.Parse("27.06.2017"),Product="IPod",Cost=350},
                new Sale {Id=Guid.NewGuid().ToString(), Client="Ivan Urgant",Date=DateTime.Parse("27.06.2017"),Product="IPhone 6+",Cost=830},
                new Sale {Id=Guid.NewGuid().ToString(), Client="Bill Parker",Date=DateTime.Parse("28.06.2017"),Product="SegaMegaDrive",Cost=350},
            };
            Manager manager7 = new Manager { UserName = "Viskub", Sales = sales7 };
            var result7 = managerRepository.Create(manager7, "8888");
            managerRepository.AddToRole(manager7.Id, roleUser.Name);
            #endregion

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
