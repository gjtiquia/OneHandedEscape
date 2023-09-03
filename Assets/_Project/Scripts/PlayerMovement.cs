using System;
using UnityEngine;
using UnityAssert = UnityEngine.Assertions.Assert;

namespace Project
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(IOnGroundChecker))]
    public class PlayerMovement : MonoBehaviour
    {
        public PlayerMovementProperties Properties => _properties;
        public Vector2 Velocity => _rigidbody.velocity;

        // TODO : Refactor with PlayerMovementPropertiesSO
        [SerializeField] private PlayerMovementProperties _properties;

        private Rigidbody2D _rigidbody;
        private IOnGroundChecker _onGroundChecker;

        private PlayerMovementState _state;
        private bool _isJumpReleased;

        public void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _onGroundChecker = GetComponent<IOnGroundChecker>();

            UnityAssert.IsNotNull(_rigidbody);
            UnityAssert.IsNotNull(_onGroundChecker);

            _state = PlayerMovementState.Idle;
            _isJumpReleased = true;
        }

        public void Update()
        {
            // TODO : Each state should implement Update and switch state on its own

            if (_state == PlayerMovementState.JumpingUp && _rigidbody.velocity.y <= 0)
                _state = PlayerMovementState.FallingDown;

            if (_onGroundChecker.IsOnGround() && IsCloseToZero(_rigidbody.velocity.y))
                _state = PlayerMovementState.Idle;
        }

        public void SetProperties(PlayerMovementProperties properties)
        {
            _properties = properties;
        }

        public void PressJump()
        {
            if (_state != PlayerMovementState.Idle) return;
            if (!_isJumpReleased) return;

            _isJumpReleased = false;
            _state = PlayerMovementState.JumpingUp;

            // TODO : Can refactor to do this OnEnterState
            _rigidbody.AddForce(Vector2.up * _properties.JumpForce, ForceMode2D.Impulse);
        }

        public void ReleaseJump()
        {
            _isJumpReleased = true;

            if (_state == PlayerMovementState.JumpingUp)
                _rigidbody.AddForce(Vector2.down * _rigidbody.velocity.y * (1 - _properties.JumpCutMultiplier), ForceMode2D.Impulse);
        }

        // TODO : Refactor into MathUtilities
        private bool IsCloseToZero(float number)
        {
            return Mathf.Abs(number) <= 0.01f;
        }
    }

    public enum PlayerMovementState
    {
        Idle,
        JumpingUp,
        FallingDown
    }

    [System.Serializable]
    public class PlayerMovementProperties
    {
        public float JumpForce = 0f;
        public float JumpCutMultiplier = 0f;
    }

    public interface IOnGroundChecker
    {
        public bool IsOnGround();
    }
}
