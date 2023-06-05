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
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace Traveling_Platform.Controllers
{
    public class HotelsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public HotelsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment env
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
        }

        public IEnumerable<SelectListItem> GetAllCities()
        {
            var selectList = new List<SelectListItem>();
            var cities = from cat in db.Cities
                         select cat;
            foreach (var city in cities)
            {
                selectList.Add(new SelectListItem
                {
                    Value = city.Id.ToString(),
                    Text = city.Name.ToString()
                });
            }
            return selectList;
        }

        public async Task<IEnumerable<SelectListItem>> GetUsersInManagerRoleAsync()
        {
            var managerRole = await _roleManager.FindByNameAsync("HotelManager");
            //if (managerRole == null) return new List<ApplicationUser>();

            var selectList = new List<SelectListItem>();
            var usersInManagerRole = await _userManager.GetUsersInRoleAsync(managerRole.Name);

            foreach (var userInManagerRole in usersInManagerRole)
            {
                selectList.Add(new SelectListItem
                {
                    Value = userInManagerRole.Id.ToString(),
                    Text = userInManagerRole.Email.ToString()
                });
            }
            return selectList;
        }


        // GET: Hotels
        public async Task<IActionResult> Index()
        {
            return db.Hotels != null ?
                        View(await db.Hotels.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Hotels'  is null.");
        }

        // GET: Hotels/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var hotel = await db.Hotels
                .Where(h => h.id_hotel == id)
                .Join(db.Cities, h => h.id_city, c => c.Id, (h, c) => new { Hotel = h, City = c })
                .Join(db.Countries, hc => hc.City.stateTag, co => co.tag, (hc, co) => new { hc.Hotel, hc.City, Country = co.commonName })
                .Join(db.Users, hc => hc.Hotel.id_manager, u => u.Id, (hc, u) => new { hc.Hotel, hc.City, hc.Country, Manager = u.Email })
                .Select(hcm => new
                {
                    Id = hcm.Hotel.id_hotel,
                    Name = hcm.Hotel.name,
                    Description = hcm.Hotel.description,
                    PhoneNumber = hcm.Hotel.PhoneNumber,
                    CityName = hcm.City.Name,
                    //Country = hcm.City.Country.officialName,
                    ManagerEmail = hcm.Manager,
                    MainImage = hcm.Hotel.Pictures.FirstOrDefault(p => p.Tag == "Main Image"),
                    us = _userManager.GetUserId(User)
                })
                .FirstOrDefaultAsync();

            if (hotel == null)
            {
                return NotFound();
            }

            var pictureData = hotel.MainImage?.Data;
            var pictureBase64 = pictureData != null ? Convert.ToBase64String(pictureData) : null;

            ViewBag.PictureBase64 = pictureBase64;
            ViewBag.Hotel = hotel;

            TempData["hotid"] = id;

            return View();
        }

        public IActionResult Testare()
        {
            return View();
        }

        // GET: Hotels/Create
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create()
        {
            HotelViewModel hotel = new HotelViewModel();
            hotel.Cit = GetAllCities();
            hotel.Man = await GetUsersInManagerRoleAsync();
            return View(hotel);
        }

        // POST: Hotels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(HotelViewModel model)
        {
            if (ModelState.IsValid)
            {
                var hotel = new Hotel
                {
                    name = model.Name,
                    description = model.Description,
                    PhoneNumber = model.PhoneNumber,
                    id_city = model.CityId,
                    id_manager = model.ManagerId
                };

                if (model.PictureFile != null && model.PictureFile.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await model.PictureFile.CopyToAsync(stream);
                        var picture = new Picture
                        {
                            FileName = model.PictureFile.FileName,
                            Data = stream.ToArray(),
                            Tag = "Main Image",
                            Hotel = hotel
                        };
                        hotel.Pictures = new List<Picture> { picture };
                    }
                }

                db.Hotels.Add(hotel);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: Hotels/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await db.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,HotelManager")]
        public async Task<IActionResult> Edit(int id, [Bind("id_hotel,name,description,PhoneNumber,id_city,id_manager")] Hotel hotel)
        {
            if (id != hotel.id_hotel)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(hotel);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.id_hotel))
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
            return View(hotel);
        }

        // GET: Hotels/Delete/5
        [Authorize(Roles = "Admin,HotelManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await db.Hotels
                .FirstOrDefaultAsync(m => m.id_hotel == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,HotelManager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Hotels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Hotels'  is null.");
            }
            var hotel = await db.Hotels.FindAsync(id);
            if (hotel != null)
            {
                db.Hotels.Remove(hotel);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(int id)
        {
            return (db.Hotels?.Any(e => e.id_hotel == id)).GetValueOrDefault();
        }
    }
}
