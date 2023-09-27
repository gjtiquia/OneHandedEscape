using UnityEngine;
using Project.Core;

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
            InputListener.OnJumpInputChanged += OnJumpInputChanged;
            InputListener.OnInputPolled += OnInputPolled;
        }

        private void OnDisable()
        {
            InputListener.OnJumpInputChanged -= OnJumpInputChanged;
            InputListener.OnInputPolled -= OnInputPolled;
        }

        private void OnJumpInputChanged(bool isJumpPressed)
        {
            if (isJumpPressed)
                _playerMovement.PressJump();
            else
                _playerMovement.ReleaseJump();
        }

        private void OnInputPolled()
        {
            if (InputListener.IsRightPressed)
                _playerMovement.PressRight();

            if (InputListener.IsLeftPressed)
                _playerMovement.PressLeft();
        }
    }
}
