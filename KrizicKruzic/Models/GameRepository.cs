using KrizicKruzic.Interfaces;

namespace KrizicKruzic.Models
{
    public class GameRepository : IGameRepository
    {
        private readonly GameDBContext _dbContext;

        public GameRepository(GameDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        IEnumerable<Game> IGameRepository.GetGames()
        {
            return _dbContext.Games.ToList();
        }
    }
}
