using System.ComponentModel.DataAnnotations;

namespace StatisticSystem.PL.Models
{
    public class SaleModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage ="Fill the field")]
        [RegularExpression(@"\d\d.\d\d.\d\d\d\d", ErrorMessage = "Please enter valid date")]
        public string Date { get; set; }

        [Required(ErrorMessage = "Fill the field")]
        public string Client { get; set; }

        [Required(ErrorMessage = "Fill the field")]
        public string Product { get; set; }

        [Required(ErrorMessage = "Fill the field")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid cost")]
        public string Cost { get; set; }

        public string ManagerId { get; set; }
    }
}