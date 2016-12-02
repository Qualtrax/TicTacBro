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

        public void Join()
        {
            if (game == null)
                game = new Game();

            var emptyGame = new Char[9];
            var moveEvents = game.Events.Where(e => e is MoveEvent).Cast<MoveEvent>();
            var movesList = moveEvents.Select(move => move.Position).ToList();

            Clients.Client(Context.ConnectionId).InitializeBoard(movesList);
        }

        public void Start()
        {
            game = new Game();
            Clients.All.InitializeBoard(new NewGameFactory().BuildEmptyGame());
        }

        public void YourTurnBro(Int32 position, Char player)
        {
            var nextPlayer = PlayerFactory.CreatePlayer(player);
            game.MakeMove(nextPlayer, position);

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
    }
}