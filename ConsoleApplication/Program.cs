using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StatisticSystem.BLL.DTO;
using StatisticSystem.BLL.Services;
using StatisticSystem.DAL.EF;
using StatisticSystem.DAL.Entities;
using StatisticSystem.DAL.Repositories;
using StatisticSystem.PL.Utills;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ManagersDataBaseConnection"].ConnectionString;

            //var result = new List<PieChartItem>();
            //result.Add(new PieChartItem { Name = "Ukraine", Value = 8 });
            //result.Add(new PieChartItem { Name = "Russia", Value = 6 });
            //result.Add(new PieChartItem { Name = "Belarus", Value = 6 });
            //result.Add(new PieChartItem { Name = "USA", Value = 4 });
            var result = new Dictionary<string, int>()
            {
                {"Rome", 5},
                { "Spain", 6},
                {"Britain", 7 }
            };
            var serilizer = new JavaScriptSerializer();
            var res = serilizer.Serialize(result);

            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }

    }
}