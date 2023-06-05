using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ReviewsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Reviews
        /*
        public IActionResult Index(int? id)
        {
            if(id == null)
            {
                return View(db.Reviews.ToList());
            }
            var hotel = db.Hotels.Find(id);
            ViewBag.Nume = hotel.name;

            var reviews=new List<ReviewViewModel>();
            foreach (Review rev in db.Reviews.Where(r => r.IdHotel == id).ToList())
            { ReviewViewModel review = new ReviewViewModel();
                review.Id = rev.Id;
                review.Time = rev.Time;
                review.Text = rev.Text;
                review.ClientName = db.Users.Find(rev.IdClient).FirstName + " " + db.Users.Find(rev.IdClient).LastName;
                review.HotelName = db.Hotels.Find(rev.IdHotel).name;
                reviews.Add(review);
            }
            ViewBag.revs = reviews;
            return View();
        } */

        // GET: Reviews
        [Authorize (Roles = "Admin, HotelManager, User, HotelReceptionist")]
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


        // GET: Reviews/Details/5
        [Authorize(Roles = "Admin, HotelManager, User, HotelReceptionist")]
        public IActionResult Details(int? id)
        {
            if (id == null || db.Reviews == null)
            {
                return NotFound();
            }

            ReviewViewModel review = new ReviewViewModel();
            review.Time = db.Reviews.Find(id).Time;
            review.Text=db.Reviews.Find(id).Text;
            review.ClientName = db.Users.Find(db.Reviews.Find(id).IdClient).FirstName + " "+ db.Users.Find(db.Reviews.Find(id).IdClient).LastName;
            review.HotelName =db.Hotels.Find(( db.Reviews.Find(id).IdHotel)).name;


            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        [Authorize(Roles = "Admin, HotelManager, User, HotelReceptionist")]
        public IActionResult Create()
        {
            return RedirectToAction("Index");
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Review review)
        {
            if (ModelState.IsValid)
            {
                review.Time= DateTime.Now;
                db.Add(review);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { id = review.IdHotel });
            }
            else { return Redirect("/Hotels/Details/" + review.IdHotel);
            }
        }

        // GET: Reviews/Edit/5
        [Authorize(Roles = "Admin, HotelManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Reviews == null)
            {
                return NotFound();
            }

            var review = await db.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, HotelManager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,Time,IdClient,IdHotel")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(review);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
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
            return View(review);
        }

        // GET: Reviews/Delete/5
        [Authorize(Roles = "Admin, HotelManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Reviews == null)
            {
                return NotFound();
            }

            var review = await db.Reviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, HotelManager, User, HotelReceptionist")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Reviews == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reviews'  is null.");
            }
            var review = await db.Reviews.FindAsync(id);
            if (review != null)
            {
                db.Reviews.Remove(review);
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { id = review.IdHotel });
        }

        private bool ReviewExists(int id)
        {
          return (db.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
