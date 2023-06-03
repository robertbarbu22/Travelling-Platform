using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Traveling_Platform.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        public DateTime Checkin { get; set; }

        public DateTime Checkout { get; set; }

        public string IdUser { get; set; }

        [NotMapped]
        public ApplicationUser? Client { get; set; }

        public int IdHotel { get; set; }
        [NotMapped]
        public Hotel? Hotel { get; set;}

        public int IdRoom { get; set; }

        [NotMapped]
        public Room? Room { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Rooms { get; set; }

    }
}
