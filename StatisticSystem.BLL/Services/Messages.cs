using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSystem.BLL.Services
{
    public static class Messages
    {
        public static string SuccessManagerAdd
        {
            get
            {
                return "Manager add to service";
            }
        }

        public static string ErrorManagerAdd
        {
            get
            {
                return "Unable to add manager. Inner error in ";
            }
        }

        public static string ManagerExist
        {
            get
            {
                return "Manager already exist with name ";
            }
        }

        public static string SuccessManagerDelete
        {
            get
            {
                return "Manager successfully deleted";
            }
        }

        public static string ErrorManagerDelete
        {
            get
            {
                return "Unable to delete manager. Inner error in ";
            }
        }

        public static string SuccessUpdateSale
        {
            get
            {
                return "Sale update in data base";
            }
        }

        public static string ErrorUpdateSale
        {
            get
            {
                return "Unable to update sale. Inner error in ";
            }
        }

        public static string SuccessDeleteeSale
        {
            get
            {
                return "Sale delete from data base";
            }
        }

        public static string ErrorDeleteSale
        {
            get
            {
                return "Unable to delete sale. Inner error in ";
            }
        }



    }
}
