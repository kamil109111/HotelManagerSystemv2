using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagerSystemv2.Areas.Admin.Models
{
    public class Room
    {        
        public int RoomId { get; set; }
        public int RoomNumber { get; set; }
        public string RoomImage { get; set; }
        public int RoomPrice { get; set; }
        public int RoomCapacity { get; set; }
        public string RoomDescription { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public int RoomStatusId { get; set; }
    }
}
