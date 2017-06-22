using StatisticSystem.BLL.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatisticSystem.PL.Models
{
    public class IndexViewModel
    {
        public IEnumerable<ManagerProfileDTO> ManagerProfiles { get; set; }
        public IEnumerable<SaleDTO> Sales { get; set; }
        public PageInfo PageInfo { get; set; }

        public string Id { get; set; }
    }
}