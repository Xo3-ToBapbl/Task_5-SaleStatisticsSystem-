using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StatisticSystem.PL.Models
{
    public class ManagerModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage ="Fill the field")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "UserName must be at least 4 characters and not more than 10")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Fill the field")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Password must be at least 4 characters and not more than 10")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Fill the field")]
        public string Role { get; set; }
    }
}