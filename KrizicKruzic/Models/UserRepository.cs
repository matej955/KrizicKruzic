using KrizicKruzic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KrizicKruzic.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly GameDBContext _dbContext;

        public UserRepository(GameDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool AddUser(User user)
        {
            try
            {
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsUserRegistered(GameDBContext dbContext, string username)
        {
            // Provjeri je li korisnik registriran u bazi podataka
            var user = dbContext.Users.FirstOrDefault(u => u.Username == username);

            // Ako je korisnik pronađen, znači da je registriran
            return user != null;
        }

        public bool ValidateCredentials(string username, string password)
        {
            using (var dbContext = _dbContext)
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Username == username);

                {
                    // Korisnik nije pronađen u bazi
                    return false;
                }

                // Provjeri lozinku korisnika
                return user.Password == password;
            }
        }

        public User GetUser(string username)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Username == username );
        }

        public Player GetPlayerByUsername(string username)
        {
            return _dbContext.Players.FirstOrDefault(p => p.Name == username);
        }
    }
}
