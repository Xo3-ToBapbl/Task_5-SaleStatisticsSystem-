using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatisticSystem.PL.Utill
{
    public static class DropDownLists
    {
        public static IEnumerable<string> Roles
        {
            get
            {
                return new List<string> { "user", "admin" };
            }
        }

        public static IEnumerable<string> Filtres
        {
            get
            {
                return new List<string> { "Date", "Client", "Product" };
            }
        }

        public static IEnumerable<string> SaleFiltres
        {
            get
            {
                return new List<string> { "None", "Date", "Client", "Product" };
            }
        }
    }
}