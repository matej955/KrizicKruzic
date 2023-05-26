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
            return null;
            //return _dbContext.Games.ToList();
        }

        public void AddGame(Game game)
        {
            //_dbContext.Games.Add(game);
            _dbContext.SaveChanges();
        }
    }
}
