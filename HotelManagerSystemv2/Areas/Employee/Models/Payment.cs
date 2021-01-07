using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagerSystemv2.Areas.Employee.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string Info { get; set; }
        public double Amount { get; set; }
        public Booking Booking { get; set; }
        public int BookingId { get; set; }

    }
}
