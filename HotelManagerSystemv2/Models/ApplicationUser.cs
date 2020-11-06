using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagerSystemv2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
        public string Photo { get; set; }
        public string Login { get; set; }
    }
}
