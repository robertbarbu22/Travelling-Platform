using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Traveling_Platform.Models
{
    public class CountryViewModel
    {
        public string tag { get; set; }
        public string commonName { get; set; }
        public string officialName { get; set; }
        public IFormFile Clickbait { get; set; }
    }
}
