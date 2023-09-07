using System;
using UnityEngine;

namespace Project
{
    public class InputListener : MonoBehaviour
    {
        public static bool IsRightPressed => _isRightPressed;
        public static bool IsLeftPressed => _isLeftPressed;
        public static event Action<bool> OnJumpInputChanged;
        public static event Action OnInputPolled;

        private static bool _isRightPressed;
        private static bool _isLeftPressed;
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
        [SerializeField] private InputButton _leftUIButton;
        [SerializeField] private InputButton _rightUIButton;
        [SerializeField] private InputButton _upLeftUIButton;
        [SerializeField] private InputButton _upRightUIButton;

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
            _isLeftPressed = HasLeftInput();
            _isRightPressed = HasRightInput();

            OnInputPolled?.Invoke();
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
            return _jumpUIButton.IsPressed() || _upLeftUIButton.IsPressed() || _upRightUIButton.IsPressed();
        }

        private bool HasLeftInput()
        {
            return HasKeyboardLeftInput() || HasUILeftInput();
        }

        private bool HasKeyboardLeftInput()
        {
            return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        }

        private bool HasUILeftInput()
        {
            return _leftUIButton.IsPressed() || _upLeftUIButton.IsPressed();
        }

        private bool HasRightInput()
        {
            return HasKeyboardRightInput() || HasUIRightInput();
        }

        private bool HasKeyboardRightInput()
        {
            return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        }

        private bool HasUIRightInput()
        {
            return _rightUIButton.IsPressed() || _upRightUIButton.IsPressed();
        }
    }
}