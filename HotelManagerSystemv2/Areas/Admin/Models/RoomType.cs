using System.ComponentModel.DataAnnotations;

namespace HotelManagerSystemv2.Areas.Admin.Models
{
    public class RoomType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Podaj nazwę")]
        [Display(Name = "Nazwa standardu")]
        public string RoomTypeName { get; set; }

        [Display(Name = "Łazienka")]
        public bool Bathroom { get; set; }

        [Display(Name = "Suszarka")]
        public bool Hairdryer { get; set; }

        [Display(Name = "Ręczniki")]
        public bool Towels { get; set; }

        [Display(Name = "Prysznic")]
        public bool Shower { get; set; }

        [Display(Name = "Wanna")]
        public bool Bath { get; set; }

        [Display(Name = "Jacuzzi")]
        public bool Jacuzzi { get; set; }

        [Display(Name = "Bidet")]
        public bool Bidet { get; set; }


        [Display(Name = "Toaleta")]
        public bool Toilet { get; set; }

        [Display(Name = "Telewizor")]
        public bool TV { get; set; }

        [Display(Name = "Klimatyzacja")]
        public bool AirConditioning { get; set; }

        [Display(Name = "Biurko")]
        public bool Desk { get; set; }

        [Display(Name = "Szlafroki kąpielowe")]
        public bool Bathrobes { get; set; }

        [Display(Name = "Taras")]
        public bool Terrace { get; set; }

        [Display(Name = "Balkon")]
        public bool Balcony { get; set; }

        [Display(Name = "Lodówka")]
        public bool Fridge { get; set; }

        [Display(Name = "Kuchnia")]
        public bool Kitchen { get; set; }

        [Display(Name = "Aneks kuchenny")]
        public bool Kitchenette { get; set; }

        [Display(Name = "Pralka")]
        public bool WashingMachine { get; set; }

        [Display(Name = "Żelazko")]
        public bool Iron { get; set; }

        [Display(Name = "Radio")]
        public bool Radio { get; set; }

        [Display(Name = "Internet Wi-Fi")]
        public bool Internet { get; set; }
    }
}
