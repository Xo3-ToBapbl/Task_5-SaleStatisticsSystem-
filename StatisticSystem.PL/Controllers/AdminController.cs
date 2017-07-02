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
            if (filterValue == "")
            {
                ViewBag.Message = "Please, enter data.";
                return PartialView();
            }
            else
            {
                DateTime date;
                if ((filter == "Date" && DateTime.TryParse(filterValue, out date)) || filter != "Date")
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
                else
                {
                    ViewBag.Message = "Please, enter valid date.";
                    return PartialView();
                }
            }           
        }    
        
        #endregion

        #region DeleteManager

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
                    ViewBag.Message = "You can not delete yourself, administrators or a users with an undefined role. Please, call servicemanager.";
                    return PartialView();
                }
                else
                {
                    return PartialView(managerModel);
                }
            }
            else
            {
                ViewBag.Message = "There is no manager to delete.";
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

        #endregion

        #region AddNewUser

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult AddNewManager()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewManager(ManagerModel model)
        {           
            if (ModelState.IsValid)
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<ManagerModel, ManagerDTO>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.SecondName));
                });
                ManagerDTO managerDTO = Mapper.Map<ManagerModel, ManagerDTO>(model);               
                OperationDetails details = ServiceBLL.Add(managerDTO);
                ViewBag.Message = details.Message;
                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        #endregion

        #region Sales

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Sales(string managerId, string message="")
        {
            ViewBag.Message = message;
            ViewBag.ManagerId = managerId;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult FiltredSales(string managerId, string filter, string filterValue)
        {
            if (filter != "None" && filterValue == "")
            {
                ViewBag.Message = "Please, enter data.";
                return PartialView();
            }
            else
            {
                DateTime date;
                if ((filter == "Date" && DateTime.TryParse(filterValue, out date)) || filter != "Date")
                {
                    SaleCollectionModel model = GetSaleCollectionModel(managerId, filter, filterValue);
                    if (model.Sales.Count() != 0)
                    {
                        return PartialView(model);
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
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult EditSale(string id, string managerId)
        {
            SaleDTO saleDTO = ServiceBLL.GetSale(id);
            if (saleDTO != null)
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<SaleDTO, SaleModel>();
                });
                SaleModel model = Mapper.Map<SaleDTO, SaleModel>(saleDTO);
                return View(model);
            }
            else
            {
                SaleModel model = new SaleModel { ManagerId = managerId };
                ViewBag.ErrorMessage = "There is no sale to edit";
                return View(model);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult EditSale(SaleModel saleModel)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<SaleModel, SaleDTO>();
                });
                SaleDTO saleDTO = Mapper.Map<SaleModel, SaleDTO>(saleModel);
                OperationDetails detail = ServiceBLL.UpdateSale(saleDTO);
                ViewBag.Message = detail.Message;               
            }
            else
            {
                return View(saleModel);
            }
            return View(saleModel);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteSale(string id)
        {           
            return PartialView();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteSale(string id, string managerId)
        {
            OperationDetails detail = ServiceBLL.DeleteSale(id);
            return RedirectToAction("Sales", new { message= detail.Message, managerId = managerId });
        }

        #endregion


        public JsonResult GetProducts()
        {
            //Dictionary<DateTime, int> data = ServiceBLL.GetDateSalesCount(managerId);
            //List<PieChartItem> result = new List<PieChartItem>();
            //data.Keys.ToList().ForEach(x => result.Add(new PieChartItem { Name = x.ToString("d"), Value = data[x] }));
            var result = new List<PieChartItem>();
            result.Add(new PieChartItem { Name = "Ukraine", Value = 8 });
            result.Add(new PieChartItem { Name = "Russia", Value = 6 });
            result.Add(new PieChartItem { Name = "Belarus", Value = 6 });
            result.Add(new PieChartItem { Name = "USA", Value = 4 });
            return Json(new { Dates = result }, JsonRequestBehavior.AllowGet);
        }

        private SaleCollectionModel GetSaleCollectionModel(string id, string filter="None", string filterValue="None")
        {
            IEnumerable<SaleDTO> salesDTO = ServiceBLL.GetSalesByManager(id, filter, filterValue);
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SaleDTO, SaleModel>();
            });
            IEnumerable<SaleModel> sales = Mapper.Map<IEnumerable<SaleDTO>, IEnumerable<SaleModel>>(salesDTO);
            return new SaleCollectionModel { Sales = sales };
        }

        private Dictionary<SaleModel, string> GetFiltredSales(string filter, string filterValue)
        {
            Dictionary<SaleDTO, string> filtredSalesDTO = ServiceBLL.GetFiltredSales(filter, filterValue);
            Mapper.Initialize(cfg =>
                    {
                        cfg.CreateMap<SaleDTO, SaleModel>();
                    });   
            return Mapper.Map<Dictionary<SaleDTO, string>, Dictionary<SaleModel, string>>(filtredSalesDTO);
        }
    }
}