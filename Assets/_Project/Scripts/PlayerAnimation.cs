using UnityEngine;
using UnityAssert = UnityEngine.Assertions.Assert;

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

        private void OnValidate()
        {
            UnityAssert.IsTrue(_horizontalSpeedThreshold >= 0);

            UnityAssert.IsNotNull(_playerMovement);
            UnityAssert.IsNotNull(_animator);
            UnityAssert.IsNotNull(_displayTransform);
        }

        private void Update()
        {
            UpdateDisplayDirection();
            UpdateAnimatorFloats();
        }

        private void UpdateDisplayDirection()
        {
            // Assumes is originally facing right
            Vector3 displayScale = _displayTransform.localScale;

            if (_playerMovement.Velocity.x > 0)
            {
                displayScale.x = Mathf.Abs(displayScale.x);
            }
            else if (_playerMovement.Velocity.x < 0)
            {
                displayScale.x = -1 * Mathf.Abs(displayScale.x);
            }

            _displayTransform.localScale = displayScale;
        }

        private void UpdateAnimatorFloats()
        {
            bool hasHorizontalSpeed = Mathf.Abs(_playerMovement.Velocity.x) > _horizontalSpeedThreshold;
            bool hasVerticalVelocity = Mathf.Abs(_playerMovement.Velocity.y) > 0;

            _animator.SetBool("HasHorizontalSpeed", hasHorizontalSpeed);
            _animator.SetBool("HasVerticalVelocity", hasVerticalVelocity);
        }
    }
}