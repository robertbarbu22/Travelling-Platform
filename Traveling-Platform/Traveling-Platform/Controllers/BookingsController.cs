using System;
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
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var adminbookings = from book in db.Bookings
                               select new
                               {
                                   bookdate = book.BookingDate,
                                   checkin = book.Checkin,
                                   checkout = book.Checkout,
                                   email = db.Users.Find(book.IdUser).Email,
                                   hotel = db.Hotels.Find(book.IdHotel).name,
                                   room = db.Rooms.Find(book.IdRoom).Name
                               };
            }

            //ViewBag.Utilizator = db.Users.Find()
            var bookings = db.Bookings.ToList();
            //ViewBag.bookings = bookings;
            foreach(var book in bookings)
            {
                //ViewData[(string)book.Id]= db.Users.Find(book.IdUser);
            }

            return View(bookings);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Bookings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookingDate,Checkin,Checkout,IdUser,IdHotel,IdRoom")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.Client = db.Users.Find(booking.IdUser); 
                db.Add(booking);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
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
    }
}
