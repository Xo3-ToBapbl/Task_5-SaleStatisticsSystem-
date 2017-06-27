﻿using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using StatisticSystem.BLL.DTO;
using StatisticSystem.BLL.Interfaces;
using StatisticSystem.BLL.Services;
using StatisticSystem.PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StatisticSystem.PL.Utill;

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

        public JsonResult GetProducts(string managerId)
        {
            Dictionary<DateTime, int> data = ServiceBLL.GetDateSalesCount(managerId);
            List<PieChartItem> result = new List<PieChartItem>();
            data.Keys.ToList().ForEach(x => result.Add(new PieChartItem { Name = x.ToString("d"), Value = data[x] }));
            return Json(new { Dates = result }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin")]
        public ActionResult AdminPage()
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
        [Authorize(Roles = "admin")]
        public ActionResult AddNewManager()
        {
            ViewBag.Message = "Please fill all fields to add manager";
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult DetailStatistics()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult FiltredStatistic(string filter, string filterValue)
        {
            Dictionary<SaleModel, string> filtredSalesModel = new Dictionary<SaleModel, string>();
            if (filter=="Date")
            {
                DateTime date;
                if (DateTime.TryParse(filterValue, out date))
                {
                    filtredSalesModel = GetFiltredSales(filter, filterValue);
                }
                else
                {
                    ViewBag.Message = "Please, enter valid date";
                }                  
            }
            else
            {
                filtredSalesModel = GetFiltredSales(filter, filterValue);
            }           
            return PartialView(filtredSalesModel);
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
                    OperationDetails details = await ServiceBLL.Add(managerDTO);
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
        [Authorize(Roles = "admin")]
        public ActionResult Sales(string ManagerId)
        {
            SaleCollectionModel model = GetSaleCollectionModel(ManagerId);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
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

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult EditSale(string id, string client, string date, string product, string cost, string managerId)
        {
            SaleModel saleModel = new SaleModel { Id = id, Client = client, Date = date, Product = product, Cost = cost, ManagerId= managerId };
            return View(saleModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult EditSale(SaleModel saleModel)
        {
            if (ModelState.IsValid)
            {
                DateTime dateValue;
                if (!DateTime.TryParse(saleModel.Date, out dateValue))
                {
                    ViewBag.Message = "Current date is not exist.";
                }
                else
                {
                    Mapper.Initialize(cfg =>
                    {
                        cfg.CreateMap<SaleModel, SaleDTO>().
                        ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Parse(src.Date))).
                        ForMember(dest => dest.Cost, opt => opt.MapFrom(src => Convert.ToInt32(src.Cost)));
                    });
                    SaleDTO saleDTO = Mapper.Map<SaleModel, SaleDTO>(saleModel);
                    OperationDetails detail = ServiceBLL.UpdateSale(saleDTO);
                    ViewBag.Message = detail.Message;
                }                 
            }
            else
            {
                return View(saleModel);
            }
            return View(saleModel);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteSale(string id, string client, string date, string product, string cost, string managerId)
        {
            ViewBag.Message = "Are Your sure? Delete this sale?";
            SaleModel saleModel = new SaleModel { Id = id, Client = client, Date = date, Product = product, Cost = cost, ManagerId = managerId };
            return View(saleModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteSale(SaleModel saleModel)
        {
            OperationDetails detail = ServiceBLL.DeleteSale(saleModel.Id);
            ViewBag.Message = detail.Message;
            return View(saleModel);
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

        private Dictionary<SaleModel, string> GetFiltredSales(string filter, string filterValue)
        {
            Dictionary<SaleDTO, string> filtredSalesDTO = ServiceBLL.GetFiltredSales(filter, filterValue);
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SaleDTO, SaleModel>().
                ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("d"))).
                ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost.ToString()));
            });
            return Mapper.Map<Dictionary<SaleDTO, string>, Dictionary<SaleModel, string>>(filtredSalesDTO);
        }
    }
}