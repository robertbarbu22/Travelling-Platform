using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Traveling_Platform.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set;}

        public bool? IsReceptionist { get; set; }

        public bool? IsManager { get; set; }

        public int? id_hotel { get; set; }

        [NotMapped]
        public virtual Hotel? Hotel { get; set; }

        public virtual ICollection<Message>? Messages { get; set; }
        public virtual ICollection<Booking>? Bookings { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<Hotel>? Hotels { get; set; }


        [NotMapped]
        public IEnumerable<SelectListItem> AllRoles { get; set; }
    }
}
