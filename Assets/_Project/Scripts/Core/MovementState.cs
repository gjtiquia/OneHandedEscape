using UnityEngine;

namespace Project.Core
{
    public class PlayerInput
    {
        public bool IsJumpPressedDown;
        public bool IsJumpReleased;

        public bool IsRightPressed;
        public bool IsLeftPressed;

        public float JumpBufferTimer => _jumpBufferTimer;

        private float _jumpBufferTimer;

        public PlayerInput()
        {
            ClearInputs();
            _jumpBufferTimer = Mathf.Infinity; // Means that the last time jump was pressed is arbitratily long
        }

        public void ClearInputs()
        {
            IsJumpPressedDown = false;
            IsJumpReleased = false;
            IsRightPressed = false;
            IsLeftPressed = false;
        }

        public void UpdateTimers(float deltaTime)
        {
            if (IsJumpPressedDown)
                _jumpBufferTimer = 0;

            _jumpBufferTimer += deltaTime;
        }
    }

    public class MovementStateFactory
    {
        public GroundedState GroundedState;
        public JumpingUpState JumpingUpState;
        public FallingDownState FallingDownState;

        public MovementStateFactory(PlayerMovement movement)
        {
            GroundedState = new GroundedState(movement);
            JumpingUpState = new JumpingUpState(movement);
            FallingDownState = new FallingDownState(movement);
        }
    }

    public abstract class MovementState
    {
        protected PlayerMovement _movement;
        protected PlayerMovementProperties _properties => _movement.Properties;
        protected Rigidbody2D _rigidbody => _movement.Rigidbody;
        protected MovementStateFactory _stateFactory => _movement.StateFactory;

        protected bool _debugLog = false;

        public MovementState(PlayerMovement movement)
        {
            _movement = movement;
        }

        public abstract void OnEnter(PlayerInput input);
        public abstract void OnExit(PlayerInput input);
        public abstract MovementState OnFixedUpdate(PlayerInput input, float fixedDeltaTime);
    }

    public class GroundedState : MovementState
    {
        private float _elapsedTimeOffGround;

        public GroundedState(PlayerMovement movement) : base(movement)
        {
            InitializeState();
        }

        private void InitializeState()
        {
            _elapsedTimeOffGround = 0;
        }

        public override void OnEnter(PlayerInput input)
        {
            if (_debugLog) Debug.Log("Enter GroundedState");

            InitializeState();
        }

        public override void OnExit(PlayerInput input)
        {

        }

        public override MovementState OnFixedUpdate(PlayerInput input, float fixedDeltaTime)
        {
            CalculateElapsedTimeOffGround(fixedDeltaTime);

            if (IsWithinCoyoteTime() && IsWithinJumpBuffer(input.JumpBufferTimer))
                return _stateFactory.JumpingUpState;

            return this;
        }

        private void CalculateElapsedTimeOffGround(float fixedDeltaTime)
        {
            if (_movement.IsOnGround())
                _elapsedTimeOffGround = 0;
            else
                _elapsedTimeOffGround += fixedDeltaTime;
        }

        private bool IsWithinCoyoteTime()
        {
            return _elapsedTimeOffGround <= _properties.CoyoteTime;
        }

        private bool IsWithinJumpBuffer(float jumpBufferTime)
        {
            return jumpBufferTime <= _properties.JumpBuffer;
        }
    }

    public class JumpingUpState : MovementState
    {
        public JumpingUpState(PlayerMovement movement) : base(movement)
        {

        }

        public override void OnEnter(PlayerInput input)
        {
            if (_debugLog) Debug.Log("Enter JumpingUpState");

            _rigidbody.AddForce(Vector2.up * _properties.JumpForce, ForceMode2D.Impulse);
        }

        public override void OnExit(PlayerInput input)
        {

        }

        public override MovementState OnFixedUpdate(PlayerInput input, float fixedDeltaTime)
        {
            if (_rigidbody.velocity.y <= 0)
                return _stateFactory.FallingDownState;

            if (input.IsJumpReleased)
                _rigidbody.AddForce(Vector2.down * _rigidbody.velocity.y * (1 - _properties.JumpCutMultiplier), ForceMode2D.Impulse);

            return this;
        }
    }

    public class FallingDownState : MovementState
    {
        public FallingDownState(PlayerMovement movement) : base(movement)
        {

        }

        public override void OnEnter(PlayerInput input)
        {
            if (_debugLog) Debug.Log("Enter FallingDownState");

            // TODO : Increase Fall Gravity
        }

        public override void OnExit(PlayerInput input)
        {
            // TODO : Decrease Fall Gravity
        }

        public override MovementState OnFixedUpdate(PlayerInput input, float fixedDeltaTime)
        {
            if (_movement.IsOnGround() && MathUtilities.IsCloseToZero(_rigidbody.velocity.y))
                return _stateFactory.GroundedState;

            return this;
        }


    }
}