using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Traveling_Platform.Data;
using Traveling_Platform.Models;

namespace Traveling_Platform.Controllers
{
    public class CountriesController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public CountriesController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var countries = db.Countries;
            ViewBag.countries = countries;
            return View();
        }

    }
}
