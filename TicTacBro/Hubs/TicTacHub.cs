using System;
using System.Linq;
using Microsoft.AspNet.SignalR;
using TicTacBro.Domain;
using TicTacBro.Domain.Events;
using TicTacBro.Factories;
using System.Collections.Generic;

namespace TicTacBro.Hubs
{
    public class TicTacHub : Hub
    {
        private static Game game;
        private static PlayerTracker playerTracker;
        private static IPlayer nextPlayer;

        public void Join()
        {
            if (game == null)
                InitializeGame();

            var emptyGame = new Char[9];
            var moveEvents = game.Events.Where(e => e is MoveEvent).Cast<MoveEvent>();
            var movesList = moveEvents.Select(move => move.Position).ToList();

            Clients.Client(Context.ConnectionId).InitializeBoard(movesList);
        }

        public void Start()
        {
            InitializeGame();
            Clients.All.InitializeBoard(new NewGameFactory().BuildEmptyGame());
        }

        public void YourTurnBro(Int32 position)
        {
            game.MakeMove(nextPlayer, position);
            nextPlayer = playerTracker.ChangePlayer();

            var lastMove = game.Events.Last(e => e is MoveEvent) as MoveEvent;
            var gameStatus = 0;

            if (game.Events.Last() is PlayerOWonEvent)
                gameStatus = 1;
            else if (game.Events.Last() is PlayerXWonEvent)
                gameStatus = 2;
            else if (game.Events.Last() is GameEndedInATieEvent)
                gameStatus = 3;

            Clients.All.UpdateGameStatus(position, lastMove.Player.Identification(), gameStatus);
        }

        private void InitializeGame()
        {
            game = new Game();
            nextPlayer = new PlayerX();
            playerTracker = new PlayerTracker(nextPlayer);
        }
    }
}