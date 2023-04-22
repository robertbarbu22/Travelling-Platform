using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System.Drawing;

namespace Traveling_Platform.Models
{
    public class Hotel
    {
        [Key]
        public int id_hotel { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public string PhoneNumber { get; set; }

        public int id_city { get; set; }

        [NotMapped]
        public Picture MainImage { get; set; }

        public string? ImagePath { get; set; }

        [NotMapped]
        public virtual City? City { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Cities { get; set; }

        public string id_manager { get; set; }

        [NotMapped]
        public virtual ApplicationUser? Manager { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Managers { get; set; }

        public virtual ICollection<ApplicationUser>? Receptionists { get; set;}

        public virtual ICollection<Room>? Rooms { get; set;}

        public virtual ICollection<Booking>? Bookings { get; set;}

        public virtual ICollection<Review>? Reviews { get; set;}
        
        public virtual ICollection<Message>? Messages { get; set;}

        public virtual ICollection<Picture>? Pictures { get; set; }
    }
}
