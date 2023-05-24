using KrizicKruzic.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace KrizicKruzic.Controllers
{
    public class TTTController : Controller
    {
        private readonly GameRepository _gameRepository;
        private char[,] gameBoard;
        private char currentPlayer;

        public TTTController(GameRepository gameRepository)
        {
            gameBoard = new char[3, 3];
            currentPlayer = 'X';
            _gameRepository = gameRepository;
         }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/game/start")]
        public IHttpActionResult     StartGame()
        {
            gameBoard = new char[3, 3];
            currentPlayer = 'X';
            return (IHttpActionResult)Ok("Nova igra je započeta.");
        }

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
    }

    public class Move
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
