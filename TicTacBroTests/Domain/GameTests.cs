﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qualtrax.Tests.Common;
using TicTacBro.Domain;
using TicTacBro.Exceptions;
using TicTacBro.Models;

namespace TicTacBroTests.Domain
{
    [TestClass]
    public class GameTests
    {
        private Game game;

        public GameTests()
        {
            game = new Game(new GameValidator());
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
            Assert.AreEqual(player.Type(), @event.Player.Type());
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
            Assert.AreEqual(player.Type(), @event.Player.Type());
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
            Assert.AreEqual(playerX.Type(), firstEvent.Player.Type());
            Assert.AreEqual(playerX.Type(), lastEvent.Player.Type());
        }
    }
}
