using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManagerSystemv2.Areas.Admin.Models;
using HotelManagerSystemv2.Models;

namespace HotelManagerSystemv2.Areas.Employee.Models
{
    public class Booking
    {
        public string Id { get; set; }

        public DateTime FirstDay { get; set; }

        public DateTime LastDay { get; set; }

        public DateTime ReservationDate { get; set; }

        public bool Breakfast { get; set; }

        public bool Dinner { get; set; }

        public int NumberOfPeople { get; set; }

        public bool Deposit { get; set; }

        public bool AllPaid { get; set; }

        public int TotalPrice {get; set;}

        public BookingStatus BookingStatus { get; set; }
        public int BookingStatusId { get; set; }

        public ApplicationUser Guest { get; set; }
        public int GuestId { get; set; }

        public ApplicationUser Employee { get; set; }
        public int EmployeeId { get; set; }

        public Room Room { get; set; }
        public int RoomId { get; set; }

    }
}
