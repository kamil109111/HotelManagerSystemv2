using System.ComponentModel.DataAnnotations;

namespace HotelManagerSystemv2.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Podaj e-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Podaj hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
