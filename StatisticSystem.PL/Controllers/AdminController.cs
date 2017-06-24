using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using StatisticSystem.BLL.DTO;
using StatisticSystem.BLL.Interfaces;
using StatisticSystem.BLL.Services;
using StatisticSystem.PL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StatisticSystem.PL.Controllers
{
    public class AdminController : Controller
    {
        private int pageSize = 5;
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
            ViewBag.managerProfiles = ServiceBLL.GetManagerProfiles();           
            return View();
        }

        [HttpGet]
        public ActionResult AddNewManager()
        {
            ViewBag.Message = "Please fill all fields to add manager";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewManager(ManagerModel model)
        {           
            try
            {
                if (ModelState.IsValid)
                {
                    ManagerDTO managerDTO = new ManagerDTO { UserName = model.SecondName, Password = model.Password, Role = model.Role };
                    OperationDetails details = await ServiceBLL.Create(managerDTO);
                    if (details.Succedeed)
                    {
                        ViewBag.Message = details.Message;
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = details.Message;
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch(Exception)
            {
                ViewBag.Message = "Unable to add manager. Inner error.";
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> EditManager(string id)
        {
            ViewBag.Message = "";

            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ManagerDTO manager = await ServiceBLL.GetManagerById(id);
            if (manager != null)
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<ManagerDTO, ManagerModel>();
                });
                ManagerModel model = Mapper.Map<ManagerDTO, ManagerModel>(manager);
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }   
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Sales(string id)
        {
            ViewBag.Sales = ServiceBLL.GetSalesByManager(id);
            return View();
        }
    }
}