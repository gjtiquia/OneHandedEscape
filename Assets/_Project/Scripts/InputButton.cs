using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.EventSystems;

// Reference: https://forum.unity.com/threads/how-do-i-detect-when-a-button-is-being-pressed-held-on-eventtype.352368/
public class InputButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent OnButtonPress;
    public UnityEvent OnButtonRelease;

    private bool _isPointerInside;

    public void PressButton()
    {
        OnButtonPress?.Invoke();
    }

    public void ReleaseButton()
    {
        OnButtonRelease?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerInside = false;
        ReleaseButton(); // Call once when pointer exits
    }

    private void Update()
    {
        if (_isPointerInside)
        {
            PressButton(); // Call on every frame that the pointer is inside
        }
    }
}