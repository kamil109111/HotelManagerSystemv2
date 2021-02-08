using System.ComponentModel.DataAnnotations;

namespace HotelManagerSystemv2.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Podaj e-mail")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
