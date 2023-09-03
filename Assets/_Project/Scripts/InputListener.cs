using System;
using UnityEngine;

namespace Project
{
    public class InputListener : MonoBehaviour
    {
        public static event Action<bool> OnJumpInputChanged;

        private static bool _isJumpPressed
        {
            get => m_isJumpPressed;
            set
            {
                m_isJumpPressed = value;
                OnJumpInputChanged?.Invoke(m_isJumpPressed);
            }
        }
        private static bool m_isJumpPressed;

        private static InputListener _instance;

        [SerializeField] private InputButton _jumpUIButton;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            _instance = this;
            _isJumpPressed = false;
        }

        private void Update()
        {
            _isJumpPressed = HasJumpInput();
        }

        private bool HasJumpInput()
        {
            return HasKeyboardJumpInput() || HasUIJumpInput();
        }

        private bool HasKeyboardJumpInput()
        {
            return Input.GetKey(KeyCode.Space);
        }

        private bool HasUIJumpInput()
        {
            return _jumpUIButton.IsPressed();
        }
    }
}