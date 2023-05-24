using KrizicKruzic.Interfaces;
using KrizicKruzic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace KrizicKruzic.Controllers
{
    [Route("account")]
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly GameDBContext _dbContext;

        public LoginController(GameDBContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public IActionResult Login(User user)
        {
            if (user.Username == "admin" && user.Password == "admin")
            {
                // Uspješna prijava
                return Ok("Uspješno ste prijavljeni");
            }

            // Neuspješna prijava
            ModelState.AddModelError("", "Neispravno korisničko ime ili lozinka.");
            return Unauthorized(new { message = "Neuspješna prijava. Provjerite korisničko ime i lozinku." });
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] KrizicKruzic.Models.RegisterModel model)
        {
            // Provjera je li korisnik već registriran
            if (_userRepository.IsUserRegistered(_dbContext, model.User.Username))
            {
                return BadRequest(new { message = "Korisničko ime već postoji. Molimo odaberite drugo korisničko ime." });
            }

            // Kreiranje novog korisnika i spremanje u bazu podataka
            var user = new User
            {
                Username = model.User.Username,
                Password = model.User.Password
                // Dodajte ostale potrebne podatke o korisniku
            };

            _dbContext.Add(user);

            // Generiranje JWT tokena i ostale logike

            return Ok(new { message = "Registracija uspješna." });
        }

       
    }
}
