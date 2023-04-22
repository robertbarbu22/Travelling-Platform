using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Traveling_Platform.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a name!")]
        public string Name { get; set; }

        public int DoubleBedsNumber { get; set; }

        public int SingleBedsNumber { get; set; }

        public int BunkBedsNumber { get; set; }

        public bool HasBalcony { get; set; }

        public bool HasBathroom { get; set; }

        public bool HasCookingEquipment { get; set; }

        public int PricePerNight { get; set; }

        //NU UITA DE ASTA!!!!!!!!!!
        public bool IsBooked { get; set; }

        public int IdHotel { get; set; }

        public virtual Hotel? Hotel { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Hotels { get; set; }

        //public virtual ICollection<HotelRoom>? HotelRooms { get; set; }
    }
}
