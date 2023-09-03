using UnityEngine;

namespace Project
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _jumpForce;

        private Rigidbody2D _rigidbody;
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            _playerMovement = new PlayerMovement();
            // _playerMovement.OnJumpEvent += OnJumpEvent; // TODO
        }

        public void Jump()
        {
            _playerMovement.PressJump();
        }

        private void OnJumpEvent()
        {
            _rigidbody.AddForce(_jumpForce * Vector2.up, ForceMode2D.Impulse);
        }
    }
}
