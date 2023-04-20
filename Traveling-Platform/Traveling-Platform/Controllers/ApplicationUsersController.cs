using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Traveling_Platform.Models;
using Microsoft.AspNetCore.Authorization;
using Traveling_Platform.Data;
using System.Data;


namespace SchoolChatOriginal.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ApplicationUsersController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var appusers = db.Users.ToList();
            ViewBag.Appusers = appusers;
            return View();
        }

        public IActionResult Show(string id)
        {
            ApplicationUser appuser = db.Users.Find(id);
            try
            {
                string role = db.Roles.Find(db.UserRoles.Where(u => u.UserId == id).Select(u => u.RoleId).First()).ToString();
                ViewBag.Role = role;
                return View(appuser);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }

        }

        public async Task<ActionResult> MakeManager(string id)
        {
            ApplicationUser user = db.Users.Find(id);

            var roles = db.Roles.ToList();

            foreach (var role in roles)
            {
                // Scoatem userul din rolurile anterioare
                await _userManager.RemoveFromRoleAsync(user, role.Name);
            }

            await _userManager.AddToRoleAsync(user, "HotelManager");

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> MakeReceptionist(string id)
        {
            ApplicationUser user = db.Users.Find(id);

            var roles = db.Roles.ToList();

            foreach (var role in roles)
            {
                // Scoatem userul din rolurile anterioare
                await _userManager.RemoveFromRoleAsync(user, role.Name);
            }

            await _userManager.AddToRoleAsync(user, "HotelReceptionist");

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> MakeUser(string id)
        {
            ApplicationUser user = db.Users.Find(id);

            var roles = db.Roles.ToList();

            foreach (var role in roles)
            {
                // Scoatem userul din rolurile anterioare
                await _userManager.RemoveFromRoleAsync(user, role.Name);
            }

            await _userManager.AddToRoleAsync(user, "User");

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();

            var roles = from role in db.Roles
                        select role;

            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }

        public IActionResult Delete(string id)
        {
            var user = db.Users.Find(id);

            var messages = db.Messages.Where(m => m.IdSender == id);

            var reviews = db.Reviews.Where(m => m.IdClient == id); //groupr req

            var bookings = db.Bookings.Where(m => m.IdUser == id); //user groups

            if (messages != null)
            {
                var msg_list = messages.ToList();

                foreach (var message in msg_list)
                {
                    db.Messages.Remove(message);
                }
            }

            if (reviews != null)
            {
                var reviews_list = reviews.ToList();

                foreach (var r in reviews_list)
                {
                    db.Reviews.Remove(r);
                }
            }

            if (bookings != null)
            {
                var bookings_list = bookings.ToList();

                foreach (var bl in bookings_list)
                {
                    db.Bookings.Remove(bl);
                }
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);

            user.AllRoles = GetAllRoles();

            var roleNames = await _userManager.GetRolesAsync(user); // Lista de nume de roluri

            // Cautam ID-ul rolului in baza de date
            var currentUserRole = _roleManager.Roles
                                              .Where(r => roleNames.Contains(r.Name))
                                              .Select(r => r.Id)
                                              .First(); // Selectam 1 singur rol
            ViewBag.UserRole = currentUserRole;

            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, ApplicationUser newData, [FromForm] string newRole)
        {
            ApplicationUser user = db.Users.Find(id);

            user.AllRoles = GetAllRoles();

            user.UserName = newData.UserName;
            user.Email = newData.Email;
            //user.FirstName = newData.FirstName;
            //user.LastName = newData.LastName;
            user.PhoneNumber = newData.PhoneNumber;


            // Cautam toate rolurile din baza de date
            var roles = db.Roles.ToList();

            foreach (var role in roles)
            {
                // Scoatem userul din rolurile anterioare
                await _userManager.RemoveFromRoleAsync(user, role.Name);
            }
            // Adaugam noul rol selectat
            var roleName = await _roleManager.FindByIdAsync(newRole);
            await _userManager.AddToRoleAsync(user, roleName.ToString());

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
