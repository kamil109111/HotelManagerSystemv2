using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HotelManagerSystemv2.Areas.Admin.Models;
using HotelManagerSystemv2.Models;

namespace HotelManagerSystemv2.Areas.Employee.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Podaj datę początkową")]
        [DataType(DataType.Date)]        
        public DateTime FirstDay { get; set; }

        [Required(ErrorMessage = "Podaj datę końcową")]
        [DataType(DataType.Date)]        
        public DateTime LastDay { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [StringLength(500, ErrorMessage = "Notatka może mieć max. 500 znaków")]
        public string Note { get; set; }

        [Required(ErrorMessage = "Podaj imię i nazwisko")]
        [StringLength(50, ErrorMessage = "Imię i nazwisko może mieć max. 50 znaków")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Podaj numer telefonu")]        
        [Phone(ErrorMessage ="Nieprawidłowy format numeru telefonu")]
        public string Phone { get; set; }

        [Required(ErrorMessage ="Podaj adres E-mail")]        
        [EmailAddress(ErrorMessage ="Nieprawidłowy format adresu E-mail")]
        public string Email { get; set; }

        public bool Dinner { get; set; }

        [Required]
        public int NumberOfPeople { get; set; }

        public bool Deposit { get; set; }

        public bool AllPaid { get; set; }
               
        [DataType(DataType.Currency)]
        public double TotalPrice {get; set;}

        public BookingStatus BookingStatus { get; set; }        
        public int BookingStatusId { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
        public int PaymentStatusId { get; set; }

        public ApplicationUser Employee { get; set; }
        public string EmployeeId { get; set; }

        public Room Room { get; set; }
        public int RoomId { get; set; }
        

    }
}
