using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Traveling_Platform.Models;

namespace Traveling_Platform.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<HotelRoom> HotelRooms { get; set;}

        public DbSet<Message> Messages { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HotelRoom>().HasKey(hr => new {hr.Id, hr.IdRoom, hr.IdHotel});

            modelBuilder.Entity<HotelRoom>()
                .HasOne(hr => hr.Hotel)
                .WithMany(hr => hr.HotelRooms)
                .HasForeignKey(hr => hr.IdHotel);

            modelBuilder.Entity<HotelRoom>()
                .HasOne(hr => hr.Room)
                .WithMany(hr => hr.HotelRooms)
                .HasForeignKey(hr => hr.IdRoom);
            
        }
    }
}