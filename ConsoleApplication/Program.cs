using AutoMapper;
using StatisticSystem.BLL.DTO;
using StatisticSystem.BLL.Services;
using StatisticSystem.DAL.EF;
using StatisticSystem.DAL.Entities;
using StatisticSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ManagersDataBaseConnection"].ConnectionString;

            SaveObject(connectionString).Wait();

            Console.WriteLine("Press any key to close.");
            Console.ReadKey();
        }

        static async Task SaveObject(string connectionString)
        {
            using (UnitOfWork dataBase = new UnitOfWork(connectionString))
            {
                var user = new Manager() { UserName = "Stupakovic" };
                var result = await dataBase.Managers.CreateAsync(user, "9999");
                await dataBase.Managers.AddToRoleAsync(user.Id, "user");

                ManagerProfile managerProfile = new ManagerProfile {Id = user.Id, SecondName = user.UserName};
                dataBase.ManagerProfiles.Create(managerProfile);

                await dataBase.SaveAsync();
            }
        }
    }
}


//var admin = new Manager() { UserName = "Adminus" };
//var result = await dataBase.Managers.CreateAsync(admin, "1111");
//await dataBase.Roles.CreateAsync(new Role() { Name = "admin" });
//                await dataBase.Roles.CreateAsync(new Role() { Name = "user" });
//                await dataBase.Managers.AddToRoleAsync(admin.Id, "admin");

//ManagerProfile managerProfile = new ManagerProfile { Id = admin.Id, SecondName = admin.UserName };
//dataBase.ManagerProfiles.Create(managerProfile);



//var user = new Manager() { UserName = "Kulagin" };
//var result = await dataBase.Managers.CreateAsync(user, "2222");
//await dataBase.Managers.AddToRoleAsync(user.Id, "user");

//ManagerProfile managerProfile = new ManagerProfile
//{
//    Id = user.Id,
//    SecondName = user.UserName,
//    #region Sales
//    Sales = new List<Sale>()
//                    {
//                        new Sale()
//                        {
//                            Id = Guid.NewGuid().ToString(),
//                            Client = "Vasily Utkin",
//                            Date = DateTime.Now,
//                            Product = "IPhone 6",
//                            Cost = 880
//                        },
//                        new Sale()
//                        {
//                            Id = Guid.NewGuid().ToString(),
//                            Client = "Pit Pitterson",
//                            Date = DateTime.Now,
//                            Product = "IPhone 6s",
//                            Cost = 920
//                        },
//                    }
//    #endregion
//};
//dataBase.ManagerProfiles.Create(managerProfile);

//                await dataBase.SaveAsync();
