﻿using System.ComponentModel.DataAnnotations;

namespace Traveling_Platform.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a name!")]
        public string Name { get; set; }

        public int DoubleBedsNumber { get; set; }

        public int SingleBedsNumber { get; set; }

        public int BunkBedsNumber { get; set; }

        public bool HasBalcony { get; set; }

        public bool HasBathroom { get; set; }

        public bool HasCookingEquipment { get; set; }

        public virtual ICollection<HotelRoom>? HotelRooms { get; set; }
    }
}