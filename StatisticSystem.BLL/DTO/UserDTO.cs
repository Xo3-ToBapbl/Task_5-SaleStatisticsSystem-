﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSystem.BLL.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Role { get; }
    }
}
