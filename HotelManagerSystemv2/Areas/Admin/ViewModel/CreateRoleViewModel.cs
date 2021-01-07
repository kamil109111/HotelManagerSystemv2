using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagerSystemv2.Areas.Admin.ViewModel
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage ="Podaj nazwę")]
        public string RoleName { get; set; }
    }
}
