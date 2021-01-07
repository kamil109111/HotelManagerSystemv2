using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagerSystemv2.Areas.Admin.ViewModel
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }

        [Required(ErrorMessage ="Podaj nazwę użytkownika")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Podaj adres e-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage ="Podaj imię i nazwisko")]
        public string FirstNameLastName  { get; set; }

        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
    }
}
