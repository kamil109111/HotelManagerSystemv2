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

        [DataType(DataType.Date)]        
        public DateTime FirstDay { get; set; }

        [DataType(DataType.Date)]        
        public DateTime LastDay { get; set; }

        public DateTime ReservationDate { get; set; }

        public string Name { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public bool Dinner { get; set; }

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
