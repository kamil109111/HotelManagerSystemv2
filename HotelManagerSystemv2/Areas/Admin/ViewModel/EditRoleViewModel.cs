using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagerSystemv2.Areas.Admin.ViewModel
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }

        public string Id { get; set; }

        [Required(ErrorMessage = "Podaj nazwę")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
