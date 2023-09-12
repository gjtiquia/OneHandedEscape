using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Project;

namespace Project.Tests
{
    public class PlayerJumpTests : PlayerMovementTests
    {
        [Test]
        public void _00_CanDoBasicJump()
        {
            _playerMovement.PressJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0));
        }

        [Test]
        public void _01_CannotRepeatedlyJump()
        {
            _playerMovement.PressJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0));

            float initialJumpVelocity = _playerMovement.Velocity.y;

            _playerMovement.PressJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(initialJumpVelocity));
        }

        [Test]
        public void _02_ReleaseJumpButtonWhileJumpingToJumpCut()
        {
            _playerMovement.PressJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0));

            float initialJumpVelocity = _playerMovement.Velocity.y;
            _playerMovement.ReleaseJump();
            RunFixedUpdate();

            float expectedYVelocity = initialJumpVelocity * _playerMovement.Properties.JumpCutMultiplier;
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(expectedYVelocity));
        }

        [Test]
        public void _03_DoesNotJumpCutIfVelocityIsLessThanZero()
        {
            _playerMovement.PressJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0));

            _rigidbody.velocity = Vector2.down; // Mock that the player is now falling
            Assert.That(_playerMovement.Velocity.y, Is.LessThan(0));

            _playerMovement.ReleaseJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity, Is.EqualTo(Vector2.down)); // Expect no change
        }

        [Test]
        public void _04_DoesNotJumpAgainUntilReleaseJumpButton()
        {
            _playerMovement.PressJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0));

            // Mock that the player is on the ground
            _rigidbody.velocity = Vector2.zero;
            _mockOnGroundChecker.SetIsOnGround(true);
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(0));

            // Attempt to jump again (should not jump because not yet released)
            _playerMovement.PressJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(0));

            _playerMovement.ReleaseJump();
            RunFixedUpdate();
            _playerMovement.PressJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0)); // Expect no change
        }

        // TODO : May refactor with Coyote time
        [Test]
        public void _05_DoesNotJumpIfNotOnGround()
        {
            // Mock that player is not on the ground
            _mockOnGroundChecker.SetIsOnGround(false);
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(0));

            _playerMovement.PressJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(0)); // Expect no change
        }
    }
}