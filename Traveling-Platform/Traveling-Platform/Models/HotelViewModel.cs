using Microsoft.AspNetCore.Mvc.Rendering;

namespace Traveling_Platform.Models
{
    public class HotelViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PhoneNumber { get; set; }

        public int CityId { get; set; }

        public string ManagerId { get; set; }

        public IFormFile PictureFile { get; set; }

        public IEnumerable<SelectListItem>? Cit { get; set; }
        public IEnumerable<SelectListItem>? Man { get; set; }
    }

}
