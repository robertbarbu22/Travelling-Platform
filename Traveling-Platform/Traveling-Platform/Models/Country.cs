using System.ComponentModel.DataAnnotations;

namespace Traveling_Platform.Models
{
    public class Country
    {
        [Key]
        public string tag { get; set; }

        [Required(ErrorMessage = "Please provide the usual name of the country!")]
        [MaxLength(20, ErrorMessage = "Usual country name too large, please abreviate!")]
        public string commonName { get; set; }

        [Required(ErrorMessage = "Please provide the usual name of the country!")]
        [MaxLength(100, ErrorMessage = "Official country name too large, please abreviate!")]
        public string officialName { get; set;}

        public virtual ICollection<City>? Cities { get; set; }
    }
}
