using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Traveling_Platform.Data;
using Traveling_Platform.Models;

namespace Traveling_Platform.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoomsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<Hotel>> GetHotelsOfCurrentUserAsync()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                return new List<Hotel>(); // Return an empty list if the user is not authenticated.
            }

            var currentUser = await _userManager.Users.Include(u => u.Hotels).SingleOrDefaultAsync(u => u.Id == currentUserId);
            return currentUser.Hotels.ToList();
        }

        public IEnumerable<SelectListItem> GetAllHotels()
        {
            var selectList = new List<SelectListItem>();
            string id_user = _userManager.GetUserId(User);
            var hotels = from cat in db.Hotels.Where(h => h.id_manager == id_user)
                            select cat;
            foreach (var hotel in hotels)
            {
                selectList.Add(new SelectListItem
                {
                    Value = hotel.id_hotel.ToString(),
                    Text = hotel.name.ToString()
                });
            }

            return selectList;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
              return db.Rooms != null ? 
                          View(await db.Rooms.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Rooms'  is null.");
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Rooms == null)
            {
                return NotFound();
            }

            var room = await db.Rooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        [Authorize(Roles = "HotelManager, Admin")]
        public IActionResult Create()
        {
            Room room = new Room();
            room.Hotels = GetAllHotels();
            return View(room);
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HotelManager, Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,DoubleBedsNumber,SingleBedsNumber,BunkBedsNumber,HasBalcony,HasBathroom,HasCookingEquipment,PricePerNight,IdHotel")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Add(room);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Rooms/Edit/5
        [Authorize(Roles = "HotelManager, Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Rooms == null)
            {
                return NotFound();
            }

            var room = await db.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HotelManager, Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DoubleBedsNumber,SingleBedsNumber,BunkBedsNumber,HasBalcony,HasBathroom,HasCookingEquipment,PricePerNight,IdHotel")] Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(room);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
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
            return View(room);
        }

        // GET: Rooms/Delete/5
        [Authorize(Roles = "HotelManager, Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Rooms == null)
            {
                return NotFound();
            }

            var room = await db.Rooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HotelManager, Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Rooms == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Rooms'  is null.");
            }
            var room = await db.Rooms.FindAsync(id);
            if (room != null)
            {
                db.Rooms.Remove(room);
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
          return (db.Rooms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
