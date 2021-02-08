using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelManagerSystemv2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
        public string Photo { get; set; }
        [Required(ErrorMessage = ("Podaj nazwę użytkownika"))]
        public string FirstNameLastName { get; set; }
        public bool IsGuest { get; set; }
    }
}
