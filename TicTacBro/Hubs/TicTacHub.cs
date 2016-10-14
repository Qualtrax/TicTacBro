using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace TicTacBro.Hubs
{
    public class TicTacHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void CheckMe(string name, string message)
        {
            Clients.All.allGood(name, message);
        }
    }
}