using Microsoft.EntityFrameworkCore;

namespace KrizicKruzic.Models
{
    public class GameDBContext : DbContext
    {
        public GameDBContext(DbContextOptions<GameDBContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
    }
}
