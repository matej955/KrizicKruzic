using Microsoft.EntityFrameworkCore;

namespace KrizicKruzic.Models
{
    public class GameDBContext : DbContext
    {
        public GameDBContext(DbContextOptions<GameDBContext> options)
            : base(options)
        {
        }

        //public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
    }
}
