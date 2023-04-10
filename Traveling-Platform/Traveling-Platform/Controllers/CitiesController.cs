using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Traveling_Platform.Data;
using Traveling_Platform.Models;

namespace Traveling_Platform.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public CitiesController(
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
            var cities = db.Cities;
            ViewBag.cities=cities;
            return View();
        }
    }
}
