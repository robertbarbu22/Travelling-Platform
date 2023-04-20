using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Traveling_Platform.Models
{
    public class Hotel
    {
        //de inclus cumva poze
        [Key]
        public int id_hotel { get; set; }

        [Required]
        public string name { get; set; }

        public string description { get; set; }

        public string PhoneNumber { get; set; }

        public int id_city { get; set; }

        [NotMapped]
        public virtual City? City { get; set; }

        public string id_manager { get; set; }

        [NotMapped]
        public virtual ApplicationUser? Manager { get; set; }

        public virtual ICollection<ApplicationUser>? Receptionists { get; set;}

        public virtual ICollection<Room>? Rooms { get; set;}

        public virtual ICollection<Booking>? Bookings { get; set;}

        public virtual ICollection<Review>? Reviews { get; set;}
        
        public virtual ICollection<Message>? Messages { get; set;}

        public virtual ICollection<Image>? Images { get; set; }
    }
}
