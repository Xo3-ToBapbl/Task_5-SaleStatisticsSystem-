using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatisticSystem.PL.Utill
{
    public static class DropDownLists
    {
        public static IEnumerable<string> Filtres
        {
            get
            {
                return new List<string> { "Date", "Client", "Product" };
            }
        }
    }
}