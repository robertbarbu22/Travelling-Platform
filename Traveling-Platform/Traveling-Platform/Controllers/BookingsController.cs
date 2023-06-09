﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Traveling_Platform.Data;
using Traveling_Platform.Models;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;

namespace Traveling_Platform.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public BookingsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;
        }

        // GET: Bookings
        [Authorize(Roles = "Admin,HotelManager,User")]
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var adminbookings = from book in db.Bookings
                                    select new
                                    {
                                        Id = book.Id,
                                        bookdate = book.BookingDate,
                                        checkin = book.Checkin,
                                        checkout = book.Checkout,
                                        email = (from usr in db.Users where usr.Id == book.IdUser select usr.Email).First(),
                                        hotel = (from hot in db.Hotels where hot.id_hotel == book.IdHotel select hot.name).First(),
                                        city = (from cit in db.Cities
                                                where (cit.Id ==
                                                (from hot in db.Hotels where hot.id_hotel == book.IdHotel select hot.id_city).First())
                                                select cit.Name).First(),
                                        room = (from rom in db.Rooms where rom.Id == book.IdRoom select rom.Name).First()
                                    };

                var lista = adminbookings.ToList();

                ViewBag.Lista = lista;

                return View();
            }


            if (User.IsInRole("HotelManager"))
            {
                var adminbookings = from book in db.Bookings.Where(b => (db.Hotels.Where(h => b.IdHotel == h.id_hotel).First().id_manager) == _userManager.GetUserId(User))
                                    select new
                                    {
                                        Id = book.Id,
                                        bookdate = book.BookingDate,
                                        checkin = book.Checkin,
                                        checkout = book.Checkout,
                                        email = (from usr in db.Users where usr.Id == book.IdUser select usr.Email).First(),
                                        hotel = (from hot in db.Hotels where hot.id_hotel == book.IdHotel select hot.name).First(),
                                        city = (from cit in db.Cities
                                                where (cit.Id ==
                                                (from hot in db.Hotels where hot.id_hotel == book.IdHotel select hot.id_city).First())
                                                select cit.Name).First(),
                                        room = (from rom in db.Rooms where rom.Id == book.IdRoom select rom.Name).First()
                                    };

                var lista = adminbookings.ToList();

                ViewBag.Lista = lista;

                return View();
            }

            if (User.IsInRole("User"))
            {
                var adminbookings = from book in db.Bookings.Where(b => b.IdUser == _userManager.GetUserId(User))
                                    select new
                                    {
                                        Id = book.Id,
                                        bookdate = book.BookingDate,
                                        checkin = book.Checkin,
                                        checkout = book.Checkout,
                                        email = (from usr in db.Users where usr.Id == book.IdUser select usr.Email).First(),
                                        hotel = (from hot in db.Hotels where hot.id_hotel == book.IdHotel select hot.name).First(),
                                        city = (from cit in db.Cities
                                                where (cit.Id ==
                                                (from hot in db.Hotels where hot.id_hotel == book.IdHotel select hot.id_city).First())
                                                select cit.Name).First(),
                                        room = (from rom in db.Rooms where rom.Id == book.IdRoom select rom.Name).First()
                                    };

                var lista = adminbookings.ToList();

                ViewBag.Lista = lista;

                return View();
            }

            return View();
        }

        /*
         public IActionResult Index(int? id)
        {
            if (id == null)
            {
                return View(db.Reviews.ToList());
            }

            var hotel = db.Hotels.Find(id);
            ViewBag.Nume = hotel.name;

            var reviews = new List<ReviewViewModel>();
            foreach (Review rev in db.Reviews.Where(r => r.IdHotel == id).ToList())
            {
                ReviewViewModel review = new ReviewViewModel();
                review.Id = rev.Id;
                review.Time = rev.Time;
                review.Text = rev.Text;
                review.ClientName = db.Users.Find(rev.IdClient).FirstName + " " + db.Users.Find(rev.IdClient).LastName;
                review.HotelName = db.Hotels.Find(rev.IdHotel).name;
                reviews.Add(review);
            }

            ViewBag.revs = reviews;
            return View(); // Pass the 'reviews' list to the view
        }
         */

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Booking book = db.Bookings.Find(id);
            Booking b = new Booking();
            b.Id = book.Id;
            b.BookingDate = book.BookingDate;
            b.Checkin = book.Checkin;
            b.Checkout = book.Checkout;
            b.IdUser = book.IdUser;
            b.Hotel = db.Hotels.Find(book.IdHotel);
            b.Room = db.Rooms.Find(book.IdRoom);

            return View(b);
        }

        /* if (id == null || db.Bookings == null)
         {
             return NotFound();
         }

         var booking = await db.Bookings
             .FirstOrDefaultAsync(m => m.Id == id);
         if (booking == null)
         {
             return NotFound();
         }

         return View(booking);
     */

        // GET: Bookings/Create
        public IActionResult Create()
        {
            int hotidul = (int)TempData["hotid"];
            Booking book = new Booking();
            book.Rooms = GetAllRooms(hotidul);
            ViewBag.UserId = _userManager.GetUserId(User);
            ViewBag.HotelId = hotidul;
            return View(book);
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookingDate,Checkin,Checkout,IdUser,IdHotel,IdRoom")] Booking book)
        {
            if (ModelState.IsValid)
            {
                db.Add(book);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }


        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Bookings == null)
            {
                return NotFound();
            }

            var booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookingDate,Checkin,Checkout,IdUser,IdHotel,IdRoom")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(booking);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Bookings == null)
            {
                return NotFound();
            }

            var booking = await db.Bookings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Bookings == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bookings'  is null.");
            }
            var booking = await db.Bookings.FindAsync(id);
            if (booking != null)
            {
                db.Bookings.Remove(booking);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return (db.Bookings?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IEnumerable<SelectListItem> GetAllRooms(int id)
        {
            var selectList = new List<SelectListItem>();
            var rooms = from cat in db.Rooms.Where(r => r.IdHotel == id)
                        select cat;
            foreach (var room in rooms)
            {
                selectList.Add(new SelectListItem
                {
                    Value = room.Id.ToString(),
                    Text = room.Name.ToString()
                });
            }

            return selectList;
        }
    }
}
