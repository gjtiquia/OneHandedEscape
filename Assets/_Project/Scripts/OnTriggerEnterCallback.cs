using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project
{
    public class OnTriggerEnterCallback : MonoBehaviour
    {
        [SerializeField] private List<string> _targetTags;
        [SerializeField] private UnityEvent _onTriggerEnter2DEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_targetTags.Contains(other.tag)) _onTriggerEnter2DEvent?.Invoke();
        }
    }
}