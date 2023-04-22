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

namespace Traveling_Platform.Controllers
{
    public class CountriesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        public CountriesController(
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

        // GET: Countries
        public async Task<IActionResult> Index()
        {
              return db.Countries != null ? 
                          View(await db.Countries.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Countries'  is null.");
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || db.Countries == null)
            {
                return NotFound();
            }

            Country country = await db.Countries.FindAsync(id);
            country.Clickbait = db.Pictures.FirstOrDefault(p => p.Tag == id);

            if (country == null)
            {
                return NotFound();
            }

            var pictureData = country.Clickbait?.Data;
            var pictureBase64 = pictureData != null ? Convert.ToBase64String(pictureData) : null;

            ViewBag.PictureBase64 = pictureBase64;

            return View(country);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CountryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var country = new Country
                {
                    officialName = model.officialName,
                    commonName = model.commonName,
                    tag = model.tag
                };

                if (model.Clickbait != null && model.Clickbait.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await model.Clickbait.CopyToAsync(stream);
                        var picture = new Picture
                        {
                            FileName = model.Clickbait.FileName,
                            Data = stream.ToArray(),
                            Tag = model.tag,
                            //Hotel = hotel
                        };
                        country.Pictures = new List<Picture> { picture };
                    }
                }

                db.Countries.Add(country);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || db.Countries == null)
            {
                return NotFound();
            }

            var country = await db.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("tag,commonName,officialName")] Country country)
        {
            if (id != country.tag)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(country);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.tag))
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
            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || db.Countries == null)
            {
                return NotFound();
            }

            var country = await db.Countries
                .FirstOrDefaultAsync(m => m.tag == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (db.Countries == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Countries'  is null.");
            }
            var country = await db.Countries.FindAsync(id);
            if (country != null)
            {
                db.Countries.Remove(country);
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(string id)
        {
          return (db.Countries?.Any(e => e.tag == id)).GetValueOrDefault();
        }
    }
}
