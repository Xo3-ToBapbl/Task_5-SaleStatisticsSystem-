using System.Collections.Generic;

namespace StatisticSystem.BLL.DTO
{
    public class ManagerDTO
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }

        public ICollection<SaleDTO> Sales { get; set; }
    }
}
