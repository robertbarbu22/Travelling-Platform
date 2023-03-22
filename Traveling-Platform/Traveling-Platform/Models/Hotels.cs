using System.ComponentModel.DataAnnotations;

namespace Traveling_Platform.Models
{
    public class Hotels
    {

        [Key]
        public int id_hotel { get; set; }

        [Required]
        public string name { get; set; }

    }
}
