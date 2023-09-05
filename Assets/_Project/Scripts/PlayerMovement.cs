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

        private void Update()
        {
            OnUpdate(Time.deltaTime);
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
            _jumpState.OnEnter();
        }

        public void OnUpdate(float deltaTime)
        {

        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            MovementState currentJumpState = _jumpState;
            MovementState newJumpState = currentJumpState.OnFixedUpdate(_fixedUpdateInput, fixedDeltaTime);
            if (newJumpState != currentJumpState)
            {
                currentJumpState.OnExit();
                newJumpState.OnEnter();
                _jumpState = newJumpState;
            }

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

        public void PressRight()
        {
            // TODO : This should be called in OnFixedUpdate
            // TODO : Check target speed
            _rigidbody.AddForce(Vector2.right, ForceMode2D.Impulse);
        }

        public void ReleaseJump()
        {
            _fixedUpdateInput.IsJumpReleased = true;
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
