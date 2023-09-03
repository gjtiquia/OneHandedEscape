using UnityEngine;

namespace Project
{
    [RequireComponent(typeof(PlayerMovement))]
    public class InputController : MonoBehaviour
    {
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void OnEnable()
        {
            InputListener.OnInputPolledEvent += OnInput;
        }

        private void OnDisable()
        {
            InputListener.OnInputPolledEvent -= OnInput;
        }

        private void OnInput()
        {
            if (InputListener.IsJumpPressed)
                _playerMovement.PressJump();
            else
                _playerMovement.ReleaseJump();
        }
    }
}
