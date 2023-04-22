using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Traveling_Platform.Models
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }

        public string FileName { get; set; }

        public byte[] Data { get; set; }

        public string Tag { get; set; }

        public int? HotelId { get; set; }

        public virtual Hotel? Hotel { get; set; }
    }

}
