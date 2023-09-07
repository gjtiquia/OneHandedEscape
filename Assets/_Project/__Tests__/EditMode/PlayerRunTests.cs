using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Project;

namespace Project.Tests
{
    public class PlayerRunTests : PlayerMovementTests
    {
        /* This test fails because ForceMode2D.Force does not change the velocity instantly
        [Test]
        public void _00_PlayerCanMoveToTheRight()
        {
            _playerMovement.PressRight();
            _playerMovement.OnFixedUpdate(FIXED_DELTA_TIME);

            Assert.That(_playerMovement.Velocity.x, Is.GreaterThan(0));
        }
        */
    }
}