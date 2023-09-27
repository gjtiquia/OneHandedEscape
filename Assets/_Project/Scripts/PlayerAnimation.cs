using UnityEngine;
using UnityAssert = UnityEngine.Assertions.Assert;
using Project.Core;

namespace Project
{
    public class PlayerAnimation : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _horizontalSpeedThreshold;

        [Header("References")]
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _displayTransform;
        [SerializeField] private ParticleSystem _jumpParticles;

        private void OnValidate()
        {
            UnityAssert.IsTrue(_horizontalSpeedThreshold >= 0);

            UnityAssert.IsNotNull(_playerMovement);
            UnityAssert.IsNotNull(_animator);
            UnityAssert.IsNotNull(_displayTransform);
            UnityAssert.IsNotNull(_jumpParticles);
        }

        private void OnEnable()
        {
            _playerMovement.OnJumpStateChanged += OnJumpStateChanged;
        }

        private void OnDisable()
        {
            _playerMovement.OnJumpStateChanged -= OnJumpStateChanged;
        }

        private void Update()
        {
            UpdateDisplayDirection();
            UpdateAnimatorFloats();
        }

        private void OnJumpStateChanged(MovementState jumpState)
        {
            if (jumpState is JumpingUpState)
            {
                _jumpParticles.Play();
            }
        }

        private void UpdateDisplayDirection()
        {
            // Assumes is originally facing right
            Vector3 displayScale = _displayTransform.localScale;

            if (IsPlayerFacingRight())
            {
                displayScale.x = Mathf.Abs(displayScale.x);
            }
            else if (IsPlayerFacingLeft())
            {
                displayScale.x = -1 * Mathf.Abs(displayScale.x);
            }

            _displayTransform.localScale = displayScale;
        }

        private bool IsPlayerFacingLeft()
        {
            return _playerMovement.Velocity.x < -1 * _horizontalSpeedThreshold;
        }

        private bool IsPlayerFacingRight()
        {
            return _playerMovement.Velocity.x > _horizontalSpeedThreshold;
        }

        private void UpdateAnimatorFloats()
        {
            bool hasHorizontalSpeed = Mathf.Abs(_playerMovement.Velocity.x) > _horizontalSpeedThreshold;
            bool hasVerticalVelocity = Mathf.Abs(_playerMovement.Velocity.y) > 0;
            bool isOnGround = _playerMovement.IsOnGround();

            _animator.SetBool("HasHorizontalSpeed", hasHorizontalSpeed);
            _animator.SetBool("HasVerticalVelocity", hasVerticalVelocity);
            _animator.SetBool("IsOnGround", isOnGround);
        }
    }
}