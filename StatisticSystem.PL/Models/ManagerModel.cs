using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StatisticSystem.PL.Models
{
    public class ManagerModel
    {
        [Required(ErrorMessage ="Fill the field")]
        public string SecondName { get; set; }
        [Required(ErrorMessage = "Fill the field")]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }

        public static IEnumerable<string> Roles
        {
            get
            {
                return new List<string> { "user", "admin" };
            }
        }

    }
}