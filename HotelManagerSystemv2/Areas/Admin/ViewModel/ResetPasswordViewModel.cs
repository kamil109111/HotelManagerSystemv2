using System.ComponentModel.DataAnnotations;

namespace HotelManagerSystemv2.Areas.Admin.ViewModel
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Podane hasła nie pasują")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}
