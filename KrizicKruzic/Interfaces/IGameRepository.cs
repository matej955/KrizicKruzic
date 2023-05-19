using KrizicKruzic.Models;

namespace KrizicKruzic.Interfaces
{
    public interface IGameRepository
    {
        IEnumerable<Game> GetGames();
    }
}
