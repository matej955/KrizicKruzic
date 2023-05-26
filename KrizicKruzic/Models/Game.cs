namespace KrizicKruzic.Models
{
    public enum GameStatus
    {
        Open,
        InProgress,
        Finished
    }

    public class Game
    {
        public int GameId { get; set; }
        public DateTime CreatedDate { get; set; }
        public GameStatus Status { get; set; }
        public Player Winner { get; set; }
        public List<Player> Players { get; set; }
        public string GameBoard { get; set; }
        public string CurrentPlayer { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? WinnerId { get; set; }
    }


    public class Player
    {
        public string PlayerId { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }
        // Ostala svojstva igrača

        // Konstruktor
        public Player(int userId, string name)
        {
            UserId = userId;
            Name = name;
            PlayerId = new Random().Next(1,10000) + "-" + UserId;
        }
    }
}
