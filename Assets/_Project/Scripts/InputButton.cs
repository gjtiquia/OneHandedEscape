using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.EventSystems;

// Reference: https://forum.unity.com/threads/how-do-i-detect-when-a-button-is-being-pressed-held-on-eventtype.352368/
public class InputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent OnButtonPress;

    private bool isPointerPressedDown;
    private bool _isPointerInside;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerPressedDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerPressedDown = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerInside = false;
    }

    private void Update()
    {
        if (/*isPointerPressedDown && */_isPointerInside)
        {
            OnButtonPress?.Invoke();
        }
    }
}