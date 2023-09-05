using UnityEngine;

namespace Project
{
    public class PlayerInput
    {
        public bool IsJumpPressed;
        public bool IsJumpReleased;

        public PlayerInput()
        {
            Clear();
        }

        public void Clear()
        {
            IsJumpPressed = false;
            IsJumpReleased = false;
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

        public MovementState(PlayerMovement movement)
        {
            _movement = movement;
        }

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract MovementState OnFixedUpdate(PlayerInput input, float fixedDeltaTime);
    }

    public class GroundedState : MovementState
    {
        public GroundedState(PlayerMovement movement) : base(movement)
        {

        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

        public override MovementState OnFixedUpdate(PlayerInput input, float fixedDeltaTime)
        {
            if (input.IsJumpPressed)
                return _stateFactory.JumpingUpState;

            return this;
        }
    }

    public class JumpingUpState : MovementState
    {
        public JumpingUpState(PlayerMovement movement) : base(movement)
        {

        }

        public override void OnEnter()
        {
            _rigidbody.AddForce(Vector2.up * _properties.JumpForce, ForceMode2D.Impulse);
        }

        public override void OnExit()
        {

        }

        public override MovementState OnFixedUpdate(PlayerInput input, float fixedDeltaTime)
        {
            if (_rigidbody.velocity.y < 0)
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

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

        public override MovementState OnFixedUpdate(PlayerInput input, float fixedDeltaTime)
        {
            if (_movement.IsOnGround() && IsCloseToZero(_rigidbody.velocity.y))
                return _stateFactory.GroundedState;

            return this;
        }

        private bool IsCloseToZero(float number)
        {
            return Mathf.Abs(number) <= 0.01f;
        }
    }
}