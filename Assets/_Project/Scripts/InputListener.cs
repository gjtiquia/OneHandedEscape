using System;
using UnityEngine;

namespace Project
{
    public class InputListener : MonoBehaviour
    {
        public static event Action OnInputPolledEvent;

        public static bool IsJumpPressed => _isJumpPressed;
        private static bool _isJumpPressed;

        private static InputListener _instance;

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
            if (Input.GetKey(KeyCode.Space))
                _isJumpPressed = true;

            OnInputPolledEvent?.Invoke();
        }

        private void LateUpdate()
        {
            _isJumpPressed = false;
        }
    }
}