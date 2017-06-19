using StatisticSystem.BLL.DTO;
using StatisticSystem.BLL.Interfaces;
using StatisticSystem.PL.Models;
using System;
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

        public ActionResult Index()
        {
            ViewBag.Message = "Please, Log in:";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(LoginModel model)
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
                    AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claim);
                    
                    if (User.IsInRole("admin"))
                        return RedirectToAction("AdminPage");
                    else
                    {
                        return RedirectToAction("ManagerPage");
                    }
                }
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult AdminPage()
        {
            ViewBag.Name = User.Identity.Name;
            return View();
        }

        [Authorize(Roles = "user")]
        public ActionResult ManagerPage()
        {
            ViewBag.Name = User.Identity.Name;
            return View();
        }
    }
}