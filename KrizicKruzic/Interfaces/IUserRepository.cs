using KrizicKruzic.Models;

namespace KrizicKruzic.Interfaces
{
    public interface IUserRepository
    {
        bool IsUserRegistered(GameDBContext _dbContext, string username);
        bool AddUser(User user);
        bool ValidateCredentials(string username, string password);
        User GetUser(string username);
    }
}
