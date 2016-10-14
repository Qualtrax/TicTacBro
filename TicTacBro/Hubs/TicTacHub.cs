using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using TicTacBro.Domain;

namespace TicTacBro.Hubs
{
    public class TicTacHub : Hub
    {

        public void StartGame()
        {
            if (!GameStateManager.GameInProgress())
                GameStateManager.StartNewGame();
        }

        public void CheckMe(string name, string message)
        {

        }

        // receive message that square was clicked
        // send message to clients that square was clicked
        public void YourTurnBro(Int32 squareIndex)
        {
            GameStateManager.MakeMove(squareIndex);
            Clients.All.UpdateBoard(squareIndex);

            if (!GameStateManager.GameInProgress())
                Clients.All.UpdateGameStatus(GameStateManager.GetGameStateStatus());
        }
    }
}