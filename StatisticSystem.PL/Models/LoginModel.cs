using System.ComponentModel.DataAnnotations;

namespace StatisticSystem.PL.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Fill the field")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Fill the field")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}