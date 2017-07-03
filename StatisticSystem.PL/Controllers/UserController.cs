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

        private Functions Functions
        {
            get
            {
                return new Functions();
            }
        }

        [Authorize(Roles = "user")]
        public ActionResult ManagerPage(string message = "")
        {
            ViewBag.Message = message;
            ViewBag.Name = User.Identity.Name;
            IEnumerable<ManagerDTO> managersDTO = ServiceBLL.GetManagers();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ManagerDTO, ManagerModel>().ForMember(dest => dest.SecondName, opt => opt.MapFrom(src => src.UserName));
            });
            IEnumerable<ManagerModel> managersModel = Mapper.Map<IEnumerable<ManagerDTO>, IEnumerable<ManagerModel>>(managersDTO);
            return View(managersModel);
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public ActionResult DetailStatistics()
        {
            ViewBag.Message = MessagesPL.FilterMessage;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public ActionResult FiltredStatistic(string filter, string filterValue)
        {
            if (filterValue == "")
            {
                ViewBag.Message = MessagesPL.EmptyFieldMessage;
                return PartialView();
            }
            else
            {
                DateTime date;
                if ((filter == "Date" && DateTime.TryParse(filterValue, out date)) || filter != "Date")
                {
                    Dictionary<SaleModel, string> filtredSalesModel = Functions.GetFiltredSales(ServiceBLL, filter, filterValue);
                    if (filtredSalesModel.Count != 0)
                    {
                        return PartialView(filtredSalesModel);
                    }
                    else
                    {
                        ViewBag.Message = MessagesPL.EmptyDataMessage;
                        return PartialView();
                    }
                }
                else
                {
                    ViewBag.Message = MessagesPL.InvalidDateMessage;
                    return PartialView();
                }
            }
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public ActionResult Sales(string managerId, string message = "")
        {
            ViewBag.Message = message;
            ViewBag.ManagerId = managerId;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public ActionResult FiltredSales(string managerId, string filter, string filterValue)
        {
            if (filter != "None" && filterValue == "")
            {
                ViewBag.Message = MessagesPL.EmptyFieldMessage;
                return PartialView();
            }
            else
            {
                DateTime date;
                if ((filter == "Date" && DateTime.TryParse(filterValue, out date)) || filter != "Date")
                {
                    SaleCollectionModel model = Functions.GetSaleCollectionModel(ServiceBLL, managerId, filter, filterValue);
                    if (model.Sales.Count() != 0)
                    {
                        return PartialView(model);
                    }
                    else
                    {
                        ViewBag.Message = MessagesPL.EmptyDataMessage;
                        return PartialView();
                    }
                }
                else
                {
                    ViewBag.Message = MessagesPL.InvalidDateMessage;
                    return PartialView();
                }
            }
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