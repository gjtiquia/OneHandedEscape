using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Project;

namespace Project.Tests
{
    public class PlayerMovementTests
    {
        private PlayerMovement _playerMovement;
        private PlayerMovementProperties _movementProperties;

        private Rigidbody2D _rigidbody;
        private MockOnGroundChecker _mockOnGroundChecker;

        [SetUp]
        public void SetupBeforeEveryTest()
        {
            GameObject player = new GameObject();
            _rigidbody = player.AddComponent<Rigidbody2D>();
            _mockOnGroundChecker = player.AddComponent<MockOnGroundChecker>();

            _playerMovement = player.AddComponent<PlayerMovement>();

            _movementProperties = new PlayerMovementProperties();
            _movementProperties.JumpForce = 5f;
            _movementProperties.JumpCutMultiplier = 0.3f;

            _playerMovement.Awake();
            _playerMovement.SetProperties(_movementProperties);
        }

        [Test]
        public void _00_DefaultNoVelocity()
        {
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(0));
        }

        [Test]
        public void _01a_CanDoBasicJump()
        {
            _playerMovement.PressJump();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0));
        }

        [Test]
        public void _01b_CannotRepeatedlyJump()
        {
            _playerMovement.PressJump();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0));

            float initialJumpVelocity = _playerMovement.Velocity.y;

            _playerMovement.PressJump();
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(initialJumpVelocity));
        }

        [Test]
        public void _02a_ReleaseJumpButtonWhileJumpingToJumpCut()
        {
            _playerMovement.PressJump();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0));

            float initialJumpVelocity = _playerMovement.Velocity.y;
            _playerMovement.ReleaseJump();

            float expectedYVelocity = initialJumpVelocity * _playerMovement.Properties.JumpCutMultiplier;
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(expectedYVelocity));
        }

        [Test]
        public void _02b_DoesNotJumpCutIfVelocityIsLessThanZero()
        {
            _playerMovement.PressJump();
            Assert.That(_playerMovement.Velocity.y, Is.GreaterThan(0));

            _rigidbody.velocity = Vector2.down; // Mock that the player is now falling
            Assert.That(_playerMovement.Velocity.y, Is.LessThan(0));

            _playerMovement.Update();
            _playerMovement.ReleaseJump();
            Assert.That(_playerMovement.Velocity, Is.EqualTo(Vector2.down)); // Expect no change
        }
    }

    public class MockOnGroundChecker : MonoBehaviour, IOnGroundChecker
    {
        public bool OnGround = false;

        public bool IsOnGround()
        {
            return OnGround;
        }
    }
}

