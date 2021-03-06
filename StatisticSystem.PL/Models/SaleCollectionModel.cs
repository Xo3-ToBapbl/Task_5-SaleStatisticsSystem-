﻿using System.Collections.Generic;

namespace StatisticSystem.PL.Models
{
    public class SaleCollectionModel
    {
        private IEnumerable<SaleModel> _sales;

        public IEnumerable<SaleModel> Sales
        {
            get
            {
                if(_sales==null)
                {
                    return new List<SaleModel>();
                }
                else
                {
                    return _sales;
                }
            }
            set
            {
                _sales = value;
            }
        }
        public string Filter { get; set; }
        public string FilterValue { get; set; }
        public string ManagerId { get; set; }        
    }
}