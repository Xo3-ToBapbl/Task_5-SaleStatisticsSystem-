using System.Collections.Generic;

namespace StatisticSystem.PL.Utills
{
    public static class Filtres
    {
        public static IEnumerable<string> Roles
        {
            get
            {
                return new List<string> { "user", "admin" };
            }
        }

        public static IEnumerable<string> DetailStatisticFiltres
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