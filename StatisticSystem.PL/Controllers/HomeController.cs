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
using System.Web.Helpers;

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

        public JsonResult GetProducts()
        {
             var result = new List<PieChartItem>();
        result.Add(new PieChartItem { Name = "Ukraine", Value = 8 });
        result.Add(new PieChartItem { Name = "Russia", Value = 6 });
        result.Add(new PieChartItem { Name = "Belarus", Value = 6 });
        result.Add(new PieChartItem { Name = "USA", Value = 4 });
    
        return Json(new { Countries = result }, JsonRequestBehavior.AllowGet);
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
                    string userRole = claim.Claims.Last().Value;
                    if (userRole == "admin")
                        return RedirectToAction("AdminPage", "Admin");
                    else if (userRole == "user")
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
    }
}