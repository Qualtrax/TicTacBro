using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacBro.Domain;
using TicTacBro.Domain.Events;

namespace TicTacBroTests.Domain
{
    [TestClass]
    public class GameTests
    {
        private Game game;

        public GameTests()
        {
            game = new Game();
        }

        [TestMethod]
        public void GameHasNoEventsLoggedWhenCreated()
        {
            Assert.IsFalse(game.Events.Any());
        }

        [TestMethod]
        public void MoveEventIsLoggedWhenPlayerXMoveOccurs()
        {
            var position = 1;
            var player = new PlayerX();
            game.MakeMove(player, position);
            
            Assert.AreEqual(1, game.Events.Count());
            
            var @event = game.Events.First() as MoveEvent;
            Assert.AreEqual(position, @event.Position);
            Assert.AreEqual(player.Identification(), @event.Player.Identification());
        }

        [TestMethod]
        public void MoveEventIsLoggedWhenPlayerOMoveOccurs()
        {
            var position = 1;
            var player = new PlayerO();
            game.MakeMove(player, position);

            Assert.AreEqual(1, game.Events.Count());

            var @event = game.Events.First() as MoveEvent;
            Assert.AreEqual(position, @event.Position);
            Assert.AreEqual(player.Identification(), @event.Player.Identification());
        }

        [TestMethod]
        public void ErrorEventIsLoggedWhenPlayerMovesOutOfTurn()
        {
            var playerX = new PlayerX();
            game.MakeMove(playerX, 2);
            game.MakeMove(playerX, 3);

            var firstEvent = game.Events.First() as MoveEvent;
            var lastEvent = game.Events.Last() as MovedOutOfTurnEvent;
            Assert.AreEqual(2, game.Events.Count());
            Assert.AreEqual(2, firstEvent.Position);
            Assert.AreEqual(playerX.Identification(), firstEvent.Player.Identification());
            Assert.AreEqual(playerX.Identification(), lastEvent.Player.Identification());
        }

        [TestMethod]
        public void EventIsLoggedWhenPlayerOWinsTheGame()
        {
            var playerX = new PlayerX();
            var playerO = new PlayerO();

            game.MakeMove(playerX, 0);
            game.MakeMove(playerO, 3);
            game.MakeMove(playerX, 1);
            game.MakeMove(playerO, 4);
            game.MakeMove(playerX, 8);
            game.MakeMove(playerO, 5);

            var wonEvent = game.Events.Last();
            Assert.IsInstanceOfType(wonEvent, typeof(PlayerOWonEvent));
            Assert.AreEqual(7, game.Events.Count());
        }

        [TestMethod]
        public void EventIsLoggedWhenPlayerXWinsTheGame()
        {
            var playerX = new PlayerX();
            var playerO = new PlayerO();

            game.MakeMove(playerX, 0);
            game.MakeMove(playerO, 3);
            game.MakeMove(playerX, 1);
            game.MakeMove(playerO, 4);
            game.MakeMove(playerX, 2);

            var wonEvent = game.Events.Last();
            Assert.IsInstanceOfType(wonEvent, typeof(PlayerXWonEvent));
            Assert.AreEqual(6, game.Events.Count());
        }

        [TestMethod]
        public void EventIsLoggedWhenGameEndsInAnEarlyTie()
        {
            var playerX = new PlayerX();
            var playerO = new PlayerO();

            game.MakeMove(playerX, 0);
            game.MakeMove(playerO, 2);
            game.MakeMove(playerX, 1);
            game.MakeMove(playerO, 3);
            game.MakeMove(playerX, 5);
            game.MakeMove(playerO, 4);
            game.MakeMove(playerX, 6);

            var gameEndedInATieEvent = game.Events.Last();
            Assert.IsInstanceOfType(gameEndedInATieEvent, typeof(GameEndedInATieEvent));
            Assert.AreEqual(8, game.Events.Count());
        }
    }
}
