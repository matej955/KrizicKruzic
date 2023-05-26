using KrizicKruzic.Interfaces;
using KrizicKruzic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Web.Http;

namespace KrizicKruzic.Controllers
{
    public class TTTController : Controller
    {
        private readonly GameRepository _gameRepository;
        private readonly UserRepository _userRepository;
        private readonly GameDBContext _dbContext;
        private char[,] gameBoard;
        private char currentPlayer;

        public TTTController(GameRepository gameRepository, UserRepository userRepository)
        {
            gameBoard = new char[3, 3];
            currentPlayer = 'X';
            _gameRepository = gameRepository;
            _userRepository = userRepository;
         }

        [Authorize]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/game/start")]
        public IHttpActionResult StartGame()
        {
            var username = User.Identity.Name;
            Game game = new Game();

            if (!string.IsNullOrWhiteSpace(username))
            {
                var player = _userRepository.GetPlayerByUsername(User.Identity.Name);

                //game.GameBoard = new char[3, 3];
                game.CurrentPlayer = player.PlayerId;
                game.Status = GameStatus.Open;
            }
            else return (IHttpActionResult)BadRequest("Igrač nije ulogiran");

            _gameRepository.AddGame(game);
            return (IHttpActionResult)Ok("Nova igra je započeta.");
        }

        [Authorize]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/game/move")]
        public IHttpActionResult MakeMove([System.Web.Http.FromBody] Move move)
        {
            int row = move.Row;
            int col = move.Column;

            // Provjera je li potez ispravan
            if (row < 0 || row > 2 || col < 0 || col > 2 || gameBoard[row, col] != '\0')
                return (IHttpActionResult)BadRequest("Neispravan potez.");

            // Postavljanje oznake na ploču
            gameBoard[row, col] = currentPlayer;

            // Provjera pobjede
            if (CheckWinner(currentPlayer))
                return (IHttpActionResult)Ok($"Igra je završena. Pobjednik je {currentPlayer}.");

            // Provjera neriješenog rezultata
            if (IsBoardFull())
                return (IHttpActionResult)Ok("Igra je završena. Neriješeno.");

            // Promjena igrača
            currentPlayer = currentPlayer == 'X' ? 'O' : 'X';

            return (IHttpActionResult)Ok("Potez je izvršen.");
        }

        private bool CheckWinner(char player)
        {
            // Provjera redaka
            for (int i = 0; i < 3; i++)
            {
                if (gameBoard[i, 0] == player && gameBoard[i, 1] == player && gameBoard[i, 2] == player)
                    return true;
            }

            // Provjera stupaca
            for (int i = 0; i < 3; i++)
            {
                if (gameBoard[0, i] == player && gameBoard[1, i] == player && gameBoard[2, i] == player)
                    return true;
            }

            // Provjera dijagonala
            if (gameBoard[0, 0] == player && gameBoard[1, 1] == player && gameBoard[2, 2] == player)
                return true;

            if (gameBoard[0, 2] == player && gameBoard[1, 1] == player && gameBoard[2, 0] == player)
                return true;

            return false;
        }

        private bool IsBoardFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameBoard[i, j] == '\0')
                        return false;
                }
            }

            return true;
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("game")]
        public IActionResult GetAllGames(int pageNumber = 1, int pageSize = 10)
        {
            int totalItems = _dbContext.Games.Count(); // Ukupan broj igara

            // Paginacija
            var games = _dbContext.Games
                .OrderByDescending(g => g.StartTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Games = games.Select(g => new
                {
                    GameId = g.GameId,
                    StartTime = g.StartTime,
                    Status = GetGameStatus(g), // Metoda za određivanje statusa igre
                    Winner = GetWinner(g) // Metoda za određivanje pobjednika igre
                })
            };

            return Ok(result);
        }
        private string GetGameStatus(Game game)
        {
            DateTime now = DateTime.Now;

            if (game.StartTime > now)
            {
                return GameStatus.Open.ToString();
            }
            else if (game.EndTime < game.StartTime)
            {
                return GameStatus.Finished.ToString();
            }
            else
            {
                return GameStatus.InProgress.ToString();
            }
        }

        private string GetWinner(Game game)
        {
            if (game.WinnerId != null)
            {
                Player winner = _dbContext.Players.FirstOrDefault(p => p.Id == game.WinnerId);

                if (winner != null)
                {
                    return winner.Name;
                }
            }

            return "Nema pobjednika";
        }
        public class Move
        {
            public int Row { get; set; }
            public int Column { get; set; }
        }
    }
}
