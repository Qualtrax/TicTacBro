using System;
using Microsoft.AspNet.SignalR;

namespace TicTacBro.Hubs
{
    public class TicTacHub : Hub
    {

        public void StartGame()
        {
            if (!GameStateManager.GameInProgress())
                GameStateManager.StartNewGame();
            
            var boardStates = GameStateManager.GetBoardStates();
            Clients.Client(Context.ConnectionId).InitializeBoard(boardStates);
        }

        public void NewGame()
        {
            if (!GameStateManager.GameInProgress())
                GameStateManager.StartNewGame();

            var boardStates = GameStateManager.GetBoardStates();
            Clients.All.InitializeBoard(boardStates);
        }

        public void YourTurnBro(Int32 squareIndex)
        {
            var gameState = GameStateManager.MakeMove(squareIndex);
            var gameBoardStates = GameStateManager.GetBoardStates();
            var gameStatus = GameStateManager.GetGameStateStatus();

            Clients.All.UpdateGameStatus(squareIndex, gameBoardStates[squareIndex], gameStatus);
        }
    }
}