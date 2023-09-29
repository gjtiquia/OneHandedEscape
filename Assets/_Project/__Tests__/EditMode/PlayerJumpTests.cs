using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
        public void _04_DoesNotJumpAgainIfPassedJumpBuffer()
        {
            // Should enter JumpingUpState
            _playerMovement.PressJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0));

            // Should enter FallingDownState
            _rigidbody.velocity = Vector2.down;
            _mockOnGroundChecker.SetIsOnGround(false);
            RunFixedUpdate(500); // Mock passed over the jump buffer time //! Note that it may jump again if reach ground before jump buffer, dun make jumps too short
            Assert.That(_playerMovement.Velocity.y, Is.LessThan(0));

            // Mock that the player is on the ground
            // Should enter GroundedState
            _rigidbody.velocity = Vector2.zero;
            _mockOnGroundChecker.SetIsOnGround(true);
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(0));

            // Attempt to jump again while jump is still held (should not jump because passed jump buffer)
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(0));

            _playerMovement.ReleaseJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(0));

            _playerMovement.PressJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0)); // Jumped!
        }

        [Test]
        public void _05_DoesNotJumpIfNotOnGroundAndPassedCoyoteTime()
        {
            // Mock that player is not on the ground
            _mockOnGroundChecker.SetIsOnGround(false);
            RunFixedUpdate(999); // Arbitrarily long
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(0));

            _playerMovement.PressJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(0)); // Expect no change
        }

        [Test]
        public void _06_JumpsIfOnGround()
        {
            // Mock that player is on the ground
            _mockOnGroundChecker.SetIsOnGround(true);
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(0));

            _playerMovement.PressJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0));
        }

        [Test]
        public void _07_JumpsIfNotOnGroundButWithinCoyoteTime()
        {
            // Mock that player is not on the ground
            _mockOnGroundChecker.SetIsOnGround(false);
            RunFixedUpdate(3); // Arbitrarily short time
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(0));

            _playerMovement.PressJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0));
        }

        [Test]
        public void _08_CanJumpIfWasPressedBeforeReachingGroundWithinJumpBuffer()
        {
            // Mock that player is on the ground
            _mockOnGroundChecker.SetIsOnGround(true);
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(0));

            // Player should enter JumpingUpState
            _playerMovement.PressJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0));

            // Must release before pressing jump again later
            _playerMovement.ReleaseJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0));

            // Mock that the player is falling
            // Player should enter FallingDownState
            _rigidbody.velocity = Vector2.down;
            _mockOnGroundChecker.SetIsOnGround(false);
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(-1));

            // Still falling
            _playerMovement.PressJump();
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(-1));

            // Mock that the player is on the ground
            // Player should enter GroundedState
            _rigidbody.velocity = Vector2.zero;
            _mockOnGroundChecker.SetIsOnGround(true);
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(0));

            // Pressed jump within jump buffer, should jump
            // Player should enter JumpingUpState
            RunFixedUpdate();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0));
        }
    }
}