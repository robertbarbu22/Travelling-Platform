using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Traveling_Platform.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Can't send plain text!")]
        [MaxLength(2048, ErrorMessage = "Text too large!")]
        public string Text { get; set; }

        public DateTime Time { get; set; }

        public string IdClient { get; set; }

        [NotMapped]
        public ApplicationUser? Author { get; set; }

        public int IdHotel { get; set; }

        [NotMapped]
        public Hotel? Hotel { get; set; }
    }
}
