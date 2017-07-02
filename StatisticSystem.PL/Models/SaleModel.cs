using System;
using System.ComponentModel.DataAnnotations;

namespace StatisticSystem.PL.Models
{
    public class SaleModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage ="Fill the field")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Fill the field")]
        public string Client { get; set; }

        [Required(ErrorMessage = "Fill the field")]
        public string Product { get; set; }

        [Required(ErrorMessage = "Fill the field")]
        [Range(1, int.MaxValue, ErrorMessage ="Cost must be more than zero")]
        public int Cost { get; set; }

        public string ManagerId { get; set; }
    }
}