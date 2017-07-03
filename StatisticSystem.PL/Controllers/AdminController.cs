using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using StatisticSystem.BLL.DTO;
using StatisticSystem.BLL.Interfaces;
using StatisticSystem.BLL.Services;
using StatisticSystem.PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatisticSystem.PL.Utills;
using System.Threading.Tasks;

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
            ViewBag.Message = Utill.FilterMessage; 
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult FiltredStatistic(string filter, string filterValue)
        {
            if (filterValue == "")
            {
                ViewBag.Message = Utill.EmptyFieldMessage;
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
                        ViewBag.Message = Utill.EmptyDataMessage;
                        return PartialView();
                    }
                }
                else
                {
                    ViewBag.Message = Utill.InvalidDateMessage;
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
                    ViewBag.Message = Utill.ErrorDeleteUserMessage;
                    return PartialView();
                }
                else
                {
                    return PartialView(managerModel);
                }
            }
            else
            {
                ViewBag.Message = Utill.EmptyUserDeleteMessage;
                return PartialView();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteManager(string Id)
        {            
            OperationDetails details =await ServiceBLL.DeleteManager(Id);
            return RedirectToAction("AdminPage", new { message = details.GetFullMessage() });
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
        public async Task<ActionResult> AddNewManager(ManagerModel model)
        {           
            if (ModelState.IsValid)
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<ManagerModel, ManagerDTO>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.SecondName));
                });
                ManagerDTO managerDTO = Mapper.Map<ManagerModel, ManagerDTO>(model);               
                OperationDetails details =await ServiceBLL.AddManager(managerDTO);
                ViewBag.Message = details.GetFullMessage();
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
                ViewBag.ErrorMessage = Utill.EmptySaleEditMessage;
                return View(model);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSale(SaleModel saleModel)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<SaleModel, SaleDTO>();
                });
                SaleDTO saleDTO = Mapper.Map<SaleModel, SaleDTO>(saleModel);
                OperationDetails detail = await ServiceBLL.UpdateSale(saleDTO);
                ViewBag.Message = detail.GetFullMessage();               
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
        public async Task<ActionResult> DeleteSale(string id, string managerId)
        {
            OperationDetails detail =await ServiceBLL.DeleteSale(id);
            return RedirectToAction("Sales", new { message= detail.GetFullMessage(), managerId = managerId });
        }

        #endregion

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