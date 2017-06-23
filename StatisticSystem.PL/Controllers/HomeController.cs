using StatisticSystem.BLL.DTO;
using StatisticSystem.BLL.Interfaces;
using StatisticSystem.PL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace StatisticSystem.PL.Controllers
{
    public class HomeController : Controller
    {
        private IServiceBLL ServiceBLL
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IServiceBLL>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Message = "Please, Log in:";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogIn(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                ManagerDTO managerDTO = new ManagerDTO { UserName = model.UserName, Password = model.Password };
                ClaimsIdentity claim = await ServiceBLL.Authenticate(managerDTO);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Invalid login or password.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, claim);
                    if (User.IsInRole("admin"))
                        return RedirectToAction("AdminPage", "Admin");
                    else if (User.IsInRole("user"))
                    {
                        return RedirectToAction("ManagerPage");
                    }
                }
            }
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }



        //[Authorize(Roles = "admin")]
        //public ActionResult AdminPage()
        //{
        //    ViewBag.Name = User.Identity.Name;
        //    return View();
        //}

        //[Authorize(Roles = "user")]
        //public ActionResult ManagerPage(int page=1)
        //{
        //    ViewBag.Name = User.Identity.Name;

        //    int pageSize = 5;
        //    IEnumerable<ManagerProfileDTO> managerProfilesPerPage = ServiceBLL.GetSpanManagers((page - 1) * pageSize, pageSize);
        //    PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = ServiceBLL.ManagersCount };
        //    IndexViewModel indexViewModel = new IndexViewModel { PageInfo = pageInfo, ManagerProfiles = managerProfilesPerPage };
        //    return View(indexViewModel);
        //}

        //[HttpGet]
        //[Authorize(Roles = "user")]
        //public ActionResult Details(string Id, int page = 1)
        //{
        //    int pageSize = 3;
        //    IEnumerable <SaleDTO> sales = ServiceBLL.GetSalesById(Id);
        //    IEnumerable<SaleDTO> salesPerPage = sales.Skip((page - 1) * pageSize).Take(pageSize);
        //    PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = Sales.Count() };
        //    IndexViewModel indexViewModel = new IndexViewModel { PageInfo = pageInfo, Sales = salesPerPage, Id = Id };
        //    return View(indexViewModel);
        //}
    }
}