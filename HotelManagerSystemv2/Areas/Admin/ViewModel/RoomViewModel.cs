using HotelManagerSystemv2.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace HotelManagerSystemv2.Areas.Admin.ViewModel
{
    public class RoomViewModel
    {
        public IEnumerable<RoomStatus> RoomStatuses { get; set; }
        public IEnumerable<RoomType> RoomTypes { get; set; }
        public Room Room { get; set; }
        public IFormFile RoomImage { get; set; }
    }
}
