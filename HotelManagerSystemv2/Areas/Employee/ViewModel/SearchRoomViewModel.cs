using HotelManagerSystemv2.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagerSystemv2.Areas.Employee.ViewModel
{
    public class SearchRoomViewModel
    {
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; } = null;

        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; } = null;
        public int NoOfPeople { get; set; }
        public bool Dinner { get; set; }
        
        public IList<Room> Room { get; set; } = new List<Room>();

    }
}
