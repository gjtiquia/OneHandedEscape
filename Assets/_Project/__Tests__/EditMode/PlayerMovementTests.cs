using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Project.Core;

namespace Project.Tests
{
    public class PlayerMovementTests
    {
        protected const float DELTA_TIME = 1f / 150f;
        protected const float FIXED_DELTA_TIME = 1f / 50f;

        protected PlayerMovement _playerMovement;
        protected PlayerMovementProperties _movementProperties;

        protected Rigidbody2D _rigidbody;
        protected MockOnGroundChecker _mockOnGroundChecker;

        public void RunFixedUpdate()
        {
            _playerMovement.OnFixedUpdate(FIXED_DELTA_TIME);
        }

        public void RunFixedUpdate(int numberOfTicks)
        {
            for (int i = 0; i < numberOfTicks; i++)
            {
                RunFixedUpdate();
            }
        }

        [SetUp]
        public void SetupBeforeEveryTest()
        {
            GameObject player = new GameObject();

            _rigidbody = player.AddComponent<Rigidbody2D>();
            _rigidbody.velocity = Vector2.zero;

            _mockOnGroundChecker = player.AddComponent<MockOnGroundChecker>();
            _mockOnGroundChecker.SetIsOnGround(true);

            _playerMovement = player.AddComponent<PlayerMovement>();

            _movementProperties = new PlayerMovementProperties();

            _movementProperties.MoveSpeed = 10f;
            _movementProperties.MoveAcceleration = 2f;
            _movementProperties.MoveDecceleration = 2f;

            _movementProperties.JumpForce = 5f;
            _movementProperties.JumpCutMultiplier = 0.3f;

            _movementProperties.CoyoteTime = 0.2f;
            _movementProperties.JumpBuffer = 0.2f;

            _playerMovement.SetProperties(_movementProperties);
            _playerMovement.Initialize();
        }

        [Test]
        public void ____DefaultNoVelocity()
        {
            Assert.That(_playerMovement.Velocity.y, Is.EqualTo(0));
        }
    }

    public class MockOnGroundChecker : MonoBehaviour, IOnGroundChecker
    {
        private bool _isOnGround;

        public bool IsOnGround()
        {
            return _isOnGround;
        }

        public void SetIsOnGround(bool isOnGround)
        {
            _isOnGround = isOnGround;
        }
    }
}

