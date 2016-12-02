using System;
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
        private IPlayer playerX;
        private IPlayer playerO;

        public GameTests()
        {
            game = new Game();
            playerX = new PlayerX();
            playerO = new PlayerO();
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
            game.MakeMove(playerX, position);
            
            Assert.AreEqual(1, game.Events.Count());
            
            var @event = game.Events.First() as MoveEvent;
            Assert.AreEqual(position, @event.Position);
            Assert.AreEqual(playerX.Identification(), @event.Player.Identification());
        }

        [TestMethod]
        public void MoveEventIsLoggedWhenPlayerOMoveOccurs()
        {
            var position = 1;
            game.MakeMove(playerO, position);

            Assert.AreEqual(1, game.Events.Count());

            var @event = game.Events.First() as MoveEvent;
            Assert.AreEqual(position, @event.Position);
            Assert.AreEqual(playerO.Identification(), @event.Player.Identification());
        }

        [TestMethod]
        public void ErrorEventIsLoggedWhenPlayerMovesOutOfTurn()
        {
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

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExceptionThrownWhenSelectedSpaceIsAlreadySelected()
        {
            game.MakeMove(playerX, 0);
            game.MakeMove(playerO, 0);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExceptionThrownWhenPositionIsGreaterThanRange()
        {
            game.MakeMove(playerX, 9);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExceptionThrownWhenPositionIsLessThanRange()
        {
            game.MakeMove(playerX, -1);
        }
    }
}
