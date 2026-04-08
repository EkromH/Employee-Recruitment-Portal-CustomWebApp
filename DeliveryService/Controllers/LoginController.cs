using DeliveryService.Models;
using DeliveryService.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Controllers
{
    public class LoginController : Controller
    {
        public readonly UserManager<Users> userManager;
        private readonly SignInManager<Users> signInManager;

        public LoginController(UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        //Registration area open================================================================================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                Users u = new Users()
                {
                    Name = model.Name,
                    Email = model.Email,
                    NormalizedEmail = model.Email,
                    UserName = model.Email,
                    NormalizedUserName=model.Email

                };
                var res= await userManager.CreateAsync(u, model.Password);
                if(res.Succeeded)
                {
                    return RedirectToAction("Index", "Advertisement");
                }
                else
                {
                    foreach(var error in res.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            return View(model);
        }

        //Registration area Close================================================================================

        //Login area open================================================================================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
               var res= await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (res.Succeeded)
                {
                    return RedirectToAction("Index", "Advertisement");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Email ID or password");
                }
            }
            return View(model);
        }

        //Login area Close================================================================================


        public async Task<IActionResult> Logout()
        {
            if (signInManager.IsSignedIn(User))
            {
                await signInManager.SignOutAsync();
                return RedirectToAction("Login", "Login");
            }
            return NotFound();
          
        }
    }
}
