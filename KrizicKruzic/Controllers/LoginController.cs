using KrizicKruzic.Interfaces;
using KrizicKruzic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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
            var repoUser =  _userRepository.GetUser(user.Username);
            if (repoUser == null || !_userRepository.ValidateCredentials(user.Username, user.Password))
            {
                // Pogrešne vjerodajnice, vraćanje odgovora s greškom
                return Unauthorized();
            }

            // Stvori identitet korisnika
            var identity = new ClaimsIdentity(
                new[]
                {
            new Claim(ClaimTypes.Name, user.Username),
                    // Dodajte dodatne tvrdnje prema potrebama, poput uloga ili prilagođenih tvrdnji
                },
                "Bearer" // Naziv vaše sheme autentifikacije
            );

            // Postavi identitet korisnika u kontekst zahtjeva
             HttpContext.SignInAsync("Bearer", new ClaimsPrincipal(identity));

            // Uspješna prijava, vraćanje odgovora s uspjehom
            return Ok("Uspješna prijava");
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] KrizicKruzic.Models.RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                bool isUserRegistered = _userRepository.IsUserRegistered(_dbContext, model.User.Username);
                if (isUserRegistered)
                {
                    ModelState.AddModelError("", "Korisnik s tim imenom već postoji.");
                    return BadRequest(ModelState);
                }
 
                var result = _userRepository.AddUser(model.User);

                if (result)
                {
                    // Stvaranje igrača za novog korisnika
                    var player = new Player(model.User.Id, model.User.Username);  /*[ UserId =model.User.Id, Name = model.User.Username }*/
                    _dbContext.Players.Add(player);
                    //_dbContext.SaveChanges();

                    // Uspješna registracija
                    return Ok("Uspješna registracija");
                }
            }

            // Povratni rezultat u slučaju greške
            return BadRequest(ModelState);
        }

       
    }
}
