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
        public IActionResult Index()
        {
            int _perPage = 5;

            var countries = db.Countries.OrderBy(a => a.commonName);

            var search = "";

            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim(); // eliminam spatiile libere

                // Cautare in tari (Nume oficial si uzual)

                List<string> CountriesTags = db.Countries.Where
                                        (
                                         at => at.officialName.Contains(search)
                                         || at.commonName.Contains(search)
                                        ).Select(a => a.tag).ToList();


                // Cautare in orase (Content)
                List<string> SelectedCitiesStateTags = db.Cities
                                        .Where
                                        (
                                         c => c.Name.Contains(search)
                                        ).Select(c => (string)c.stateTag).ToList();

                /*List<string> CountriesIdsOfCities = db.Countries.Where(a => SelectedCitiesStateTags
                                                    .Contains((string)a.tag))
                                                    .Select(g => g.tag).ToList();*/


                // Se formeaza o singura lista formata din toate id-urile selectate anterior
                List<string> mergedIds = CountriesTags.Union(SelectedCitiesStateTags).ToList();


                // Lista articolelor care contin cuvantul cautat
                // fie in articol -> Title si Content
                // fie in comentarii -> Content
                countries = db.Countries.Where(g => mergedIds.Contains(g.tag))
                                      .OrderBy(a => a.commonName);
            }

            ViewBag.SearchString = search;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            int totalItems = countries.Count();

            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }

            var paginatedCountries = countries.Skip(offset).Take(_perPage);

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);

            ViewBag.Countries = paginatedCountries;

            if (search != "")
            {
                ViewBag.PaginationBaseUrl = "/Countries/Index/?search=" + search + "&page";
            }
            else
            {
                ViewBag.PaginationBaseUrl = "/Countries/Index/?page";
            }

            /*if (_userManager.GetUserId(User) != null)
            {
                string usr_id = _userManager.GetUserId(User).ToString();

                bool noreqsent;

                ViewBag.ModeratorInGroupIds = db.UserGroups.Where(ug => ug.IdUser == usr_id && ug.IsModerator == true)
                                                           .Select(ug => ug.IdGroup)
                                                           .ToList();

                if (db.GroupRequests.Where(gr => gr.IdUser == usr_id).Count() > 0)
                {
                    ViewBag.GroupsRequestedIds = db.GroupRequests.Where(gr => gr.IdUser == usr_id).Select(gr => gr.IdGroup).ToList();

                    noreqsent = false;
                }

                else
                {
                    noreqsent = true;
                }

                TempData["NoRequestsSent"] = noreqsent;
                //TempData["congrats_reason"] = noreqsent;
                //return RedirectToAction("Done");
            }*/

            //ViewBag.NoRequestsSent = true;

            return View();
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

            var cit = db.Cities.Where(c => c.stateTag == id).ToList();

            ViewBag.City = cit;

            return View(country);
        }

        // GET: Countries/Create

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
