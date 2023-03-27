using System.ComponentModel.DataAnnotations.Schema;

namespace Traveling_Platform.Models
{
    public class HotelRoom
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set;  }

        public int? IdHotel { get; set; }

        public int? IdRoom { get; set;  }

        public int NumberOfRooms { get; set; }

        public int PricePerNight { get; set; }

        public virtual Hotel? Hotel { get; set; }

        public virtual Room? Room { get; set; }
    }
}
