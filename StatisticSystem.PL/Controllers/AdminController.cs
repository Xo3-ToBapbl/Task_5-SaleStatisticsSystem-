using Microsoft.AspNet.Identity.Owin;
using StatisticSystem.BLL.DTO;
using StatisticSystem.BLL.Interfaces;
using StatisticSystem.PL.Models;
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
        public ActionResult AdminPage(int page = 1)
        {
            ViewBag.Name = User.Identity.Name;

            int pageSize = 5;
            KeyValuePair<int, IEnumerable<ManagerProfileDTO>> pairDTO = 
                ServiceBLL.GetManagersSpan((page - 1) * pageSize, pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = pairDTO.Key };
            IndexViewModel indexViewModel = new IndexViewModel { PageInfo = pageInfo, ManagerProfiles = pairDTO.Value };
            return View(indexViewModel);
        }
    }
}