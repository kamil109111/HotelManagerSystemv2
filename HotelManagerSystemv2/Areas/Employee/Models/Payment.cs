using HotelManagerSystemv2.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace HotelManagerSystemv2.Areas.Employee.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Dodaj opis płatności")]
        [Display(Name = "Opis")]
        public string Info { get; set; }

        [Required(ErrorMessage = "Podaj wpłacaną kwotę")]
        [Display(Name = "Kwota")]
        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        [Required]
        [Display(Name = "Data wpłaty")]
        public DateTime Date { get; set; }

        public Booking Booking { get; set; }
        public int BookingId { get; set; }

        public ApplicationUser Employee { get; set; }
        public string EmployeeId { get; set; }

    }
}
