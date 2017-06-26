using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StatisticSystem.BLL.DTO;
using StatisticSystem.BLL.Services;
using StatisticSystem.DAL.EF;
using StatisticSystem.DAL.Entities;
using StatisticSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ManagersDataBaseConnection"].ConnectionString;

            UnitOfWork context = new UnitOfWork(connectionString);
            var manager = context.Managers.FindByName("Stolen");

            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }

    }
}