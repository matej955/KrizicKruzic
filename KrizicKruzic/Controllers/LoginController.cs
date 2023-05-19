using KrizicKruzic.Models;
using Microsoft.AspNetCore.Mvc;

namespace KrizicKruzic.Controllers
{
    [Route("account")]
    public class LoginController : Controller
    {
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public IActionResult Login(User user)
        {
            // Logika za provjeru korisničkog imena i lozinke
            // ...

            if (user.Username == "admin" && user.Password == "admin")
            {
                // Uspješna prijava
                return RedirectToAction("Home", "Index");
            }

            // Neuspješna prijava
            ModelState.AddModelError("", "Neispravno korisničko ime ili lozinka.");
            return View(user);
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            // Logika za registraciju korisnika
            // ...

            return RedirectToAction("Login");
        }
    }
}
