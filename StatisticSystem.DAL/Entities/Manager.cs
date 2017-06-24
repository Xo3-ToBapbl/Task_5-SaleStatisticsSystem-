﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSystem.DAL.Entities
{
    public class Manager: IdentityUser
    {
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
