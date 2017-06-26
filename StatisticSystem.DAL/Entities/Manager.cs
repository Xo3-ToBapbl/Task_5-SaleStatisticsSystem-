using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace StatisticSystem.DAL.Entities
{
    public class Manager: IdentityUser
    {
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
