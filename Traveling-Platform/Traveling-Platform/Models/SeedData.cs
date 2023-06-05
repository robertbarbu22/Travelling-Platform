using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Traveling_Platform.Data;
using Traveling_Platform.Models;
using static System.Net.WebRequestMethods;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
        serviceProvider.GetRequiredService
        <DbContextOptions<ApplicationDbContext>>()))
        {
            if (!context.Roles.Any())
            {
                // baza de date contine deja roluri


                // CREAREA ROLURILOR IN BD
                // daca nu contine roluri, acestea se vor crea
                context.Roles.AddRange(
                new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Admin", NormalizedName = "Admin".ToUpper() },
                new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7211", Name = "HotelManager", NormalizedName = "HotelManager".ToUpper() },
                new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7212", Name = "HotelReceptionist", NormalizedName = "HotelReceptionist".ToUpper() },
                new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7213", Name = "User", NormalizedName = "User".ToUpper() }
                );


                // o noua instanta pe care o vom utiliza pentru crearea parolelor utilizatorilor
                // parolele sunt de tip hash
                var hasher = new PasswordHasher<ApplicationUser>();
                // CREAREA USERILOR IN BD
                // Se creeaza cate un user pentru fiecare rol
                context.Users.AddRange(
                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb0", // primary key
                    UserName = "admin@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "ADMIN@TEST.COM",
                    Email = "admin@test.com",
                    NormalizedUserName = "ADMIN@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "Admin1!")
                },
                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb1", // primary key
                    UserName = "manager@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "MANAGER@TEST.COM",
                    Email = "manager@test.com",
                    NormalizedUserName = "MANAGER@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "Manager1!")
                },
                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb2", // primary key
                    UserName = "receptionist@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "RECEPTIONIST@TEST.COM",
                    Email = "receptionist@test.com",
                    NormalizedUserName = "RECEPTIONIST@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "Receptionist1!")
                },
                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb3", // primary key
                    UserName = "user@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "USER@TEST.COM",
                    Email = "user@test.com",
                    NormalizedUserName = "USER@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "User1!")
                }
                );
                // ASOCIEREA USER-ROLE
                context.UserRoles.AddRange(
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb0"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb1"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7212",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb2"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7213",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb3"
                }
                );
            }
            if (!context.Countries.Any())
            {
                context.Countries.AddRange(
                  new Country
                  {
                      tag = "RO",
                      commonName = "Romania",
                      officialName = "Romania",
                      ImagePath = null
                  },
                  new Country
                  {
                      tag = "BG",
                      commonName = "Bulgaria",
                      officialName = "Bulgaria",
                      ImagePath = null
                  }
              );
            }
            if (!context.Cities.Any())
            {
                context.Cities.AddRange(
                   new City
                    {
                        //Id = 1,
                        Name = "Valcea",
                        stateTag = "RO"

                    },
                    new City
                    {
                        //Id = 2,
                        Name = "Sofia",
                        stateTag = "BG"
                    }
                    );
            }
            if(!context.Hotels.Any())
            {
                context.Hotels.AddRange(
                    new Hotel
                    {
                        //id_hotel = 1,
                        name = "Ramada",
                        description = "Preturi accesibile 2023",
                        PhoneNumber = "0748277393",
                        id_city = 1,
                        ImagePath = null,
                        id_manager = "8e445865-a24d-4543-a6c6-9443d048cdb1"

                    },
                    new Hotel
                    {
                        //id_hotel = 2,
                        name = "New Horrizons",
                        description = "city center",
                        PhoneNumber = "023485732",
                        id_city = 2,
                        ImagePath = null,
                        id_manager = "8e445865-a24d-4543-a6c6-9443d048cdb1"
                    }
                    );
            }
            if (context.Rooms.Any())
            {
                context.Rooms.AddRange(
                    new Room
                    {
                       // Id = 1,
                        Name = "double",
                        DoubleBedsNumber = 1,
                        SingleBedsNumber = 0,
                        BunkBedsNumber = 0,
                        HasBalcony = true,
                        HasBathroom = true,
                        HasCookingEquipment = true,
                        PricePerNight = 100,
                        IsBooked = false,
                        IdHotel = 1
                    },
                    new Room
                    {
                        //Id = 2,
                        Name = "double",
                        DoubleBedsNumber = 1,
                        SingleBedsNumber = 0,
                        BunkBedsNumber = 0,
                        HasBalcony = true,
                        HasBathroom = true,
                        HasCookingEquipment = true,
                        PricePerNight = 100,
                        IsBooked = false,
                        IdHotel = 2
                    },
                     new Room
                     {
                         //Id = 3,
                         Name = "family",
                         DoubleBedsNumber = 1,
                         SingleBedsNumber = 0,
                         BunkBedsNumber = 1,
                         HasBalcony = true,
                         HasBathroom = true,
                         HasCookingEquipment = true,
                         PricePerNight = 200,
                         IsBooked = false,
                         IdHotel = 1
                     }
                );
            }
            if (!context.Reviews.Any())
            {
                context.AddRange(
                    new Review
                    {
                        //Id = 1,
                        Text = "frumos",
                        Time = DateTime.Now,
                        IdClient = "8e445865-a24d-4543-a6c6-9443d048cdb3",
                        IdHotel = 1

                    },
                    new Review
                    {
                        //Id = 2,
                        Text = "frumos",
                        Time = DateTime.Now,
                        IdClient = "8e445865-a24d-4543-a6c6-9443d048cdb3",
                        IdHotel = 2

                    });
            }
            if (!context.Bookings.Any())
            {
                context.AddRange(
                    new Booking
                    {
                        //Id = 1,
                        BookingDate = DateTime.Now,
                        Checkin = DateTime.Now,
                        Checkout = DateTime.Now,
                        IdUser = "8e445865-a24d-4543-a6c6-9443d048cdb3",
                        IdHotel = 1,
                        IdRoom = 1

                    },
                    new Booking
                    {
                        //Id = 2,
                        BookingDate = DateTime.Now,
                        Checkin = DateTime.Now,
                        Checkout = DateTime.Now,
                        IdUser = "8e445865-a24d-4543-a6c6-9443d048cdb3",
                        IdHotel = 2,
                        IdRoom = 2

                    });
            }
            
            context.SaveChanges();
        }
    }
}