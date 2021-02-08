using HotelManagerSystemv2.Areas.Admin.Models;
using HotelManagerSystemv2.Areas.Employee.Models;
using HotelManagerSystemv2.Models;
using System.Collections.Generic;

namespace HotelManagerSystemv2.Areas.Employee.ViewModel
{
    public class BookingViewModel
    {
        public IEnumerable<BookingStatus> BookingStatuses { get; set; }
        public IEnumerable<PaymentStatus> PaymentStatuses { get; set; }
        public IEnumerable<ApplicationUser> Employees { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
        public Booking Booking { get; set; }
    }
}
