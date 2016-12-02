using System;
using System.Linq;
using Microsoft.AspNet.SignalR;
using TicTacBro.Domain;
using TicTacBro.Domain.Events;
using TicTacBro.Factories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicTacBro.Hubs
{
    public class TicTacHub : Hub
    {
        private readonly static Dictionary<String, IPlayer> connections = new Dictionary<String, IPlayer>();
        private static Game game;

        public override Task OnConnected()
        {
            if (connections.Count(c => c.Value.Identification() == new PlayerX().Identification()) == 0)
                connections.Add(Context.ConnectionId, new PlayerX());
            else if (connections.Count(c => c.Value.Identification() == new PlayerO().Identification()) == 0)
                connections.Add(Context.ConnectionId, new PlayerO());
            else
                connections.Add(Context.ConnectionId, new PlayerNone());

            return base.OnConnected();
        }

        public override Task OnDisconnected(Boolean stopCalled)
        {
            connections.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

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

        public void YourTurnBro(Int32 position)
        {
            var player = connections[Context.ConnectionId];
            game.MakeMove(player, position);

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