using System;
using UnityEngine;
using UnityAssert = UnityEngine.Assertions.Assert;

namespace Project.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(IOnGroundChecker))]
    public class PlayerMovement : MonoBehaviour
    {
        public event Action<MovementState> OnJumpStateChanged;

        public PlayerMovementProperties Properties => _properties;
        public Rigidbody2D Rigidbody => _rigidbody;
        public Vector2 Velocity => _rigidbody.velocity;
        public MovementStateFactory StateFactory => _movementStateFactory;

        // TODO : Refactor with PlayerMovementPropertiesSO
        [SerializeField] private PlayerMovementProperties _properties;

        private Rigidbody2D _rigidbody;
        private IOnGroundChecker _onGroundChecker;

        private PlayerInput _fixedUpdateInput;
        private MovementStateFactory _movementStateFactory;
        private MovementState _jumpState;

        // MonoBehaviour INTERFACE
        private void Awake()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate(Time.fixedDeltaTime);
        }

        // PUBLIC METHODS
        public void Initialize()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _onGroundChecker = GetComponent<IOnGroundChecker>();

            UnityAssert.IsNotNull(_rigidbody);
            UnityAssert.IsNotNull(_onGroundChecker);

            _fixedUpdateInput = new PlayerInput();
            _movementStateFactory = new MovementStateFactory(this);

            _jumpState = _movementStateFactory.GroundedState;
            _jumpState.OnEnter(_fixedUpdateInput);
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            UpdateJumpState(fixedDeltaTime);
            UpdateMovement(fixedDeltaTime);

            _fixedUpdateInput.Clear();
        }

        public void SetProperties(PlayerMovementProperties properties)
        {
            _properties = properties;
        }

        public bool IsOnGround()
        {
            return _onGroundChecker.IsOnGround();
        }

        public void PressJump()
        {
            _fixedUpdateInput.IsJumpPressed = true;
        }

        public void ReleaseJump()
        {
            _fixedUpdateInput.IsJumpReleased = true;
        }

        public void PressRight()
        {
            _fixedUpdateInput.IsRightPressed = true;
        }

        public void PressLeft()
        {
            _fixedUpdateInput.IsLeftPressed = true;
        }

        // PRIVATE METHODS
        private void UpdateJumpState(float fixedDeltaTime)
        {
            MovementState currentJumpState = _jumpState;
            MovementState newJumpState = currentJumpState.OnFixedUpdate(_fixedUpdateInput, fixedDeltaTime);
            if (newJumpState != currentJumpState)
            {
                currentJumpState.OnExit(_fixedUpdateInput);
                newJumpState.OnEnter(_fixedUpdateInput);

                OnJumpStateChanged?.Invoke(newJumpState);
                _jumpState = newJumpState;
            }
        }

        private void UpdateMovement(float fixedDeltaTime)
        {
            int moveInput = 0;

            if (_fixedUpdateInput.IsRightPressed && !_fixedUpdateInput.IsLeftPressed)
                moveInput = 1;
            else if (!_fixedUpdateInput.IsRightPressed && _fixedUpdateInput.IsLeftPressed)
                moveInput = -1;

            float targetSpeed = moveInput * _properties.MoveSpeed;
            float speedDiff = targetSpeed - _rigidbody.velocity.x;
            float acceleration = MathUtilities.IsCloseToZero(targetSpeed) ? _properties.MoveDecceleration : _properties.MoveAcceleration;
            float force = speedDiff * acceleration * fixedDeltaTime;

            _rigidbody.AddForce(force * Vector2.right);
        }
    }

    [System.Serializable]
    public class PlayerMovementProperties
    {
        [Header("Move Properties")]
        public float MoveSpeed = 0f;
        public float MoveAcceleration = 0f;
        public float MoveDecceleration = 0f;

        [Header("Jump Properties")]
        public float JumpForce = 0f;
        public float JumpCutMultiplier = 0f;
        public float CoyoteTime = 0f;
    }

    public interface IOnGroundChecker
    {
        public bool IsOnGround();
    }

    public static class MathUtilities
    {
        public static bool IsCloseToZero(float number)
        {
            return Mathf.Abs(number) <= 0.01f;
        }
    }
}
