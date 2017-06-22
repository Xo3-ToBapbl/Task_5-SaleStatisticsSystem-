using Microsoft.AspNet.Identity.Owin;
using StatisticSystem.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StatisticSystem.PL.Controllers
{
    public class AdminController : Controller
    {
        private IServiceBLL ServiceBLL
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IServiceBLL>();
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult AdminPage()
        {
            ViewBag.Name = User.Identity.Name;
            return View();
        }
    }
}