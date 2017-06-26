using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StatisticSystem.PL.Models
{
    public class ManagerModel
    {
        public string Id { get; set; }
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