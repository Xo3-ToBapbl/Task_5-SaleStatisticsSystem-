using AutoMapper;
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

        #region AdminPage

        [Authorize(Roles = "admin")]
        public ActionResult AdminPage(string message="")
        {
            ViewBag.Message = message;
            ViewBag.Name = User.Identity.Name;
            IEnumerable<ManagerDTO> managersDTO = ServiceBLL.GetManagers();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ManagerDTO, ManagerModel>().ForMember(dest=>dest.SecondName, opt=>opt.MapFrom(src=>src.UserName));
            });
            IEnumerable<ManagerModel> managersModel = Mapper.Map<IEnumerable<ManagerDTO>, IEnumerable<ManagerModel>>(managersDTO);                   
            return View(managersModel);
        }

        #endregion

        #region DetailStatistic

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult DetailStatistics()
        {
            ViewBag.Message = "Select a filter to display detailed statistics"; 
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult FiltredStatistic(string filter, string filterValue)
        {
            if (filterValue=="")
            {
                ViewBag.Message = "Please, enter data.";
                return PartialView();
            }
            else
            {
                if (filter=="Date")
                {
                    DateTime date;
                    if (DateTime.TryParse(filterValue, out date))
                    {
                        Dictionary<SaleModel, string> filtredSalesModel = GetFiltredSales(filter, filterValue);
                        if (filtredSalesModel.Count!=0)
                        {
                            return PartialView(filtredSalesModel);
                        }
                        else
                        {
                            ViewBag.Message = "There is no data for your request.";
                            return PartialView();
                        }                       
                    }
                    else
                    {
                        ViewBag.Message = "Please, enter valid date.";
                        return PartialView();
                    }
                }
                else
                {
                    Dictionary<SaleModel, string> filtredSalesModel = GetFiltredSales(filter, filterValue);
                    if (filtredSalesModel.Count != 0)
                    {
                        return PartialView(filtredSalesModel);
                    }
                    else
                    {
                        ViewBag.Message = "There is no data for your request.";
                        return PartialView();
                    }
                }
            }
        }

        #endregion

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteManager(string Id, string UserName)
        {
            ManagerDTO managerDTO = ServiceBLL.GetManagerById(Id);
            if (managerDTO!=null)
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<ManagerDTO, ManagerModel>().ForMember(dest=>dest.SecondName, opt=>opt.MapFrom(src=>src.UserName));
                });
                ManagerModel managerModel = Mapper.Map<ManagerDTO, ManagerModel>(managerDTO);
                if (managerModel.Role == null || managerModel.Role == "admin")
                {
                    ViewBag.Message = "You can not delete an administrator or a user with an undefined role. Please, call servicemanager.";
                    return PartialView();
                }
                else
                {
                    return PartialView(managerModel);
                }
            }
            else
            {
                ViewBag.Message = "There is manager to delete.";
                return PartialView();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteManager(string Id)
        {
            OperationDetails details = ServiceBLL.DeleteManager(Id);
            return RedirectToAction("AdminPage", new { message = details.Message });
        }

        #region AddNewUser

        [HttpGet]
        [Authorize(Roles = "admin")]
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

        #endregion

        #region Sales

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

        #endregion


        public JsonResult GetProducts(string managerId)
        {
            Dictionary<DateTime, int> data = ServiceBLL.GetDateSalesCount(managerId);
            List<PieChartItem> result = new List<PieChartItem>();
            data.Keys.ToList().ForEach(x => result.Add(new PieChartItem { Name = x.ToString("d"), Value = data[x] }));
            return Json(new { Dates = result }, JsonRequestBehavior.AllowGet);
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