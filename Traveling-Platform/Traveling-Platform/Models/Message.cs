using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Traveling_Platform.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Messages can't be empty!")]
        [MaxLength(1000, ErrorMessage = "Message exceedes the 1000 characters limit!")]
        public string Content { get; set; }

        public DateTime Time { get; set; }

        public string IdSender { get; set; }

        [NotMapped]
        public virtual ApplicationUser? Sender { get; set; }

        public int id_hotel { get; set; }

        [NotMapped]
        public virtual Hotel? Hotel { get; set; }

        public string IdClient { get; set; }

        [NotMapped]
        public virtual ApplicationUser? Client { get; set; }
    }
}
