using HotelManagerSystemv2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagerSystemv2.Areas.Employee.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Dodaj opis płatności")]
        [Display(Name = "Opis")]
        public string Info { get; set; }

        [Required(ErrorMessage ="Podaj wpłacaną kwotę")]
        [Display(Name ="Kwota")]
        [DataType(DataType.Currency)]        
        public double Amount { get; set; }

        [Required]
        [Display(Name ="Data wpłaty")]
        public DateTime Date { get; set; }

        public Booking Booking { get; set; }
        public int BookingId { get; set; }

        

    }
}
