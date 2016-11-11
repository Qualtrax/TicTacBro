using System;
using Microsoft.AspNet.SignalR;
using TicTacBro.Domain;
using System.Linq;
using TicTacBro.Factories;
using TicTacBro.Domain.Events;

namespace TicTacBro.Hubs
{
    public class TicTacHub : Hub
    {
        private Game game;
        
        public void StartGame()
        {
            if (game == null)
                game = new Game();
            
            var boardStates = game.States;
            Clients.Client(Context.ConnectionId).InitializeBoard(boardStates);
        }

        public void StartGameForAllPlayers()
        {
            game = new Game();

            var boardStates = game.States;
            Clients.All.InitializeBoard(boardStates);
        }

        public void YourTurnBro(String playerIdentificationToken, Int32 position)
        {
            var player = PlayerFactory.Build(playerIdentificationToken);
            game.MakeMove(player, position);
            var gameBoardStates = game.States;
            var gameStatus = 0;

            if (game.Events.Any(e => e is PlayerOWonEvent))
                gameStatus = 1;
            else if (game.Events.Any(e => e is PlayerXWonEvent))
                gameStatus = 2;

            Clients.All.UpdateGameStatus(position, gameBoardStates.ElementAt(position), gameStatus);
        }
    }
}