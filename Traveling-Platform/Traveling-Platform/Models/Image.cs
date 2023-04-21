namespace Traveling_Platform.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Picture { get; set; }
        public bool IsProfilePic { get; set; }
        public int IdHotel { get; set; }
        public virtual Hotel? Hotel { get; set; }
                
    }
}
