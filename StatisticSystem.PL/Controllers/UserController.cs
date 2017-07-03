using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using StatisticSystem.BLL.DTO;
using StatisticSystem.BLL.Interfaces;
using StatisticSystem.PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatisticSystem.PL.Utills;

namespace StatisticSystem.PL.Controllers
{
    public class UserController : Controller
    {
        private IServiceBLL ServiceBLL
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IServiceBLL>();
            }
        }

        [Authorize(Roles = "user")]
        public ActionResult ManagerPage()
        {
            ViewBag.Name = User.Identity.Name;
            IEnumerable<ManagerDTO> managersDTO = ServiceBLL.GetManagers();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ManagerDTO, ManagerModel>().ForMember(dest=>dest.SecondName, opt=>opt.MapFrom(src=>src.UserName));
            });
            IEnumerable<ManagerModel> managersModel = Mapper.Map<IEnumerable<ManagerDTO>, IEnumerable<ManagerModel>>(managersDTO);                   
            return View(managersModel);
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public ActionResult Sales(string ManagerId)
        {
            SaleCollectionModel model = GetSaleCollectionModel(ManagerId);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public ActionResult Sales(SaleCollectionModel model)
        {
            if(model.Filter=="Date")
            {
                DateTime outDate;
                if (DateTime.TryParse(model.FilterValue, out outDate))
                {
                    model = GetSaleCollectionModel(model.ManagerId, model.Filter, model.FilterValue);
                }
                else
                {
                    ViewBag.Message = "Please, enter correct date.";
                }
            }
            else
            {
                model = GetSaleCollectionModel(model.ManagerId, model.Filter, model.FilterValue);
            }
            return View(model);
        }       


        private SaleCollectionModel GetSaleCollectionModel(string id, string filter="None", string filterValue="None")
        {
            IEnumerable<SaleDTO> salesDTO = ServiceBLL.GetSalesByManager(id, filter, filterValue);
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SaleDTO, SaleModel>().
                ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("d"))).
                ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost.ToString()));
            });
            IEnumerable<SaleModel> sales = Mapper.Map<IEnumerable<SaleDTO>, IEnumerable<SaleModel>>(salesDTO);
            return new SaleCollectionModel { Sales = sales };
        }
    }
}