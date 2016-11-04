using System;
using Microsoft.AspNet.SignalR;
using TicTacBro.Domain;
using System.Linq;
using TicTacBro.Factories;

namespace TicTacBro.Hubs
{
    public class TicTacHub : Hub
    {
        private Game game;

        public TicTacHub()
        {
            game = new Game(new GameValidator());
        }

        public void StartGame()
        {
            if (!game.GameInProgress())
                game = new Game(new GameValidator());
            
            var boardStates = game.States;
            Clients.Client(Context.ConnectionId).InitializeBoard(boardStates);
        }

        public void NewGame()
        {
            if (!game.GameInProgress())
                game = new Game(new GameValidator());

            var boardStates = game.States;
            Clients.All.InitializeBoard(boardStates);
        }

        public void YourTurnBro(String playerIdentificationToken, Int32 position)
        {
            var player = PlayerFactory.Build(playerIdentificationToken);
            game.MakeMove(player, position);
            var gameBoardStates = game.States;
            var gameStatus = game.Status;

            Clients.All.UpdateGameStatus(position, gameBoardStates.ElementAt(position), gameStatus);
        }
    }
}