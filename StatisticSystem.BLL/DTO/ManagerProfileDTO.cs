using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSystem.BLL.DTO
{
    public class ManagerProfileDTO
    {
        public string Id { get; set; }

        public string SecondName { get; set; }

        public virtual ICollection<SaleDTO> Sales { get; set; }
    }
}
