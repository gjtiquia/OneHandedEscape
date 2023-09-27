using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.EventSystems;

// Reference: https://forum.unity.com/threads/how-do-i-detect-when-a-button-is-being-pressed-held-on-eventtype.352368/
namespace Project.UI
{
    public class InputButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public UnityEvent OnButtonPressDown;
        public UnityEvent OnButtonRelease;

        private bool _isPressed;

        public void PressDownButton()
        {
            _isPressed = true;
            OnButtonPressDown?.Invoke();
        }

        public void ReleaseButton()
        {
            _isPressed = false;
            OnButtonRelease?.Invoke();
        }

        public bool IsPressed()
        {
            return _isPressed;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PressDownButton();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ReleaseButton();
        }
    }
}