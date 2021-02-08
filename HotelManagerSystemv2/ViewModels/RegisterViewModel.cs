using System.ComponentModel.DataAnnotations;

namespace HotelManagerSystemv2.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Podaj e-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Podaj hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Podane hasła różnią się od siebie")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Podaj imię i nazwisko")]
        [Display(Name = "Imię i Nazwisko")]
        public string FirstNameLastName { get; set; }

        public bool IsGuest { get; set; }
    }
}
