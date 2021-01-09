using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagerSystemv2.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage ="Podaj e-mail")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
