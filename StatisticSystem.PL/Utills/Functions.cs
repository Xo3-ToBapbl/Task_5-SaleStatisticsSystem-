using AutoMapper;
using StatisticSystem.BLL.DTO;
using StatisticSystem.BLL.Interfaces;
using StatisticSystem.PL.Models;
using System.Collections.Generic;

namespace StatisticSystem.PL.Utills
{
    public class Functions
    {
        public Dictionary<SaleModel, string> GetFiltredSales(IServiceBLL serviceBLL, string filter, string filterValue)
        {
            Dictionary<SaleDTO, string> filtredSalesDTO = serviceBLL.GetFiltredSales(filter, filterValue);
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SaleDTO, SaleModel>();
            });
            return Mapper.Map<Dictionary<SaleDTO, string>, Dictionary<SaleModel, string>>(filtredSalesDTO);
        }

        public SaleCollectionModel GetSaleCollectionModel(IServiceBLL serviceBLL, string id, string filter = "None", string filterValue = "None")
        {
            IEnumerable<SaleDTO> salesDTO = serviceBLL.GetSalesByManager(id, filter, filterValue);
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SaleDTO, SaleModel>();
            });
            IEnumerable<SaleModel> sales = Mapper.Map<IEnumerable<SaleDTO>, IEnumerable<SaleModel>>(salesDTO);
            return new SaleCollectionModel { Sales = sales };
        }
    }
}