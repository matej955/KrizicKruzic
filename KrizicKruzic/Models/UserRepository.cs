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

        public void AddUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
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
            throw new NotImplementedException();
        }
    }
}
