using System.ComponentModel.DataAnnotations;

namespace HotelManagerSystemv2.Areas.Admin.ViewModel
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Podaj nazwę")]
        public string RoleName { get; set; }
    }
}
