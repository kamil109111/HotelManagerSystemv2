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
        [Required(ErrorMessage = "Podaj datę początkową")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateFrom { get; set; } = null;

        [Required(ErrorMessage = "Podaj datę końcową")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateTo { get; set; } = null;

        [Required(ErrorMessage = "Podaj liczbę osób")]
        [Range(1, 8, ErrorMessage = "Podaj wartość między 1 a 8")]
        public int? NoOfPeople { get; set; }
        public bool Dinner { get; set; }
        
        public IList<Room> Room { get; set; } = new List<Room>();

    }
}
