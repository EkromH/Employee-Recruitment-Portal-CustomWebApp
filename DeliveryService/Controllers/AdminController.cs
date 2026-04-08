using DeliveryService.Data;
using DeliveryService.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminLogin(AdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Find the admin by username
                var admin = _context.admin.FirstOrDefault(a => a.UserName == model.UserName);

                // 2. Check if admin exists and password matches
                // NOTE: Use a password hasher here! Never compare plain text.
                if (admin != null && admin.Password == model.Password)
                {
                    // Logic for creating a Session or Cookie goes here
                    return RedirectToAction("Register", "Login");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View(model);
        }
    }
}

