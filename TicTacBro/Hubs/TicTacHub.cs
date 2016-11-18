using System;
using System.Linq;
using Microsoft.AspNet.SignalR;
using TicTacBro.Domain;
using TicTacBro.Domain.Events;

namespace TicTacBro.Hubs
{
    public class TicTacHub : Hub
    {
        private static Game game;
        private static PlayerTracker playerTracker;
        private static IPlayer nextPlayer;

        public void StartGame()
        {
            if (game == null)
                InitializeGame();

            var boardStateViews = game.States.Select(s => s.Identification());
            Clients.Client(Context.ConnectionId).InitializeBoard(boardStateViews);
        }

        public void StartGameForAllPlayers()
        {
            InitializeGame();
            var boardStateViews = game.States.Select(s => s.Identification());
            Clients.All.InitializeBoard(boardStateViews);
        }

        public void YourTurnBro(Int32 position)
        {
            game.MakeMove(nextPlayer, position);
            nextPlayer = playerTracker.ChangePlayer();

            var gameBoardStates = game.States;
            var gameStatus = 0;

            if (game.Events.Last() is PlayerOWonEvent)
                gameStatus = 1;
            else if (game.Events.Last() is PlayerXWonEvent)
                gameStatus = 2;
            else if (game.Events.Last() is GameEndedInATieEvent)
                gameStatus = 3;

            Clients.All.UpdateGameStatus(position, gameBoardStates.ElementAt(position).Identification(), gameStatus);
        }

        private void InitializeGame()
        {
            game = new Game();
            nextPlayer = new PlayerX();
            playerTracker = new PlayerTracker(nextPlayer);
        }
    }
}