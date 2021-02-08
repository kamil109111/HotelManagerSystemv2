using System.ComponentModel.DataAnnotations;

namespace HotelManagerSystemv2.Areas.Admin.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Podaj numer pokoju")]
        public int RoomNumber { get; set; }

        public string RoomImage { get; set; }

        [Required(ErrorMessage = "Podaj cenę / za noc")]
        [DataType(DataType.Currency)]
        public int RoomPrice { get; set; }

        [Required(ErrorMessage = "Podaj max. ilość osób")]
        public int RoomCapacity { get; set; }

        [Required(ErrorMessage = "Podaj opis pokoju")]
        public string RoomDescription { get; set; }

        public RoomStatus RoomStatus { get; set; }
        public int RoomStatusId { get; set; }

        public RoomType RoomType { get; set; }
        public int RoomTypeId { get; set; }
    }
}
