using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Traveling_Platform.Models
{
    public class City
    {
        [Key]
        public int Id { get; set;  }

        [Required(ErrorMessage = "City name is mandatory!")]
        [MaxLength(50, ErrorMessage = "City name too large, please abreviate!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "State tag is mandatory!")]
        public string stateTag { get; set; }

        [NotMapped]
        public Country? Country { get; set; }

        public virtual ICollection<Hotel>? Hotels { get; set; }
    }
}
