using HotelManagerSystemv2.Areas.Employee.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagerSystemv2.Areas.Employee.ViewModel
{
    public class BookingStatusesViewModel
    {
        public SelectList BookingStatuses { get; set; }
        public List<Booking> Booking { get; set; }

        public string BookingStatus { get; set; }
        public string SearchingString { get; set; }
    }
}
