using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatisticSystem.DAL.Entities
{
    public class ManagerProfile
    {
        [Key]
        [ForeignKey("Manager")]
        public string Id { get; set; }

        public string SecondName { get; set; }
        
        public virtual Manager Manager { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}