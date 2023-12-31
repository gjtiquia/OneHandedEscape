using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project
{
    public class OnTriggerEnterCallback : MonoBehaviour
    {
        [SerializeField] private bool _isActive = true;
        [SerializeField] private List<string> _targetTags;
        [SerializeField] private UnityEvent _onTriggerEnter2DEvent;

        public void EnableCallback()
        {
            _isActive = true;
        }

        public void DisableCallback()
        {
            _isActive = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isActive && _targetTags.Contains(other.tag))
                _onTriggerEnter2DEvent?.Invoke();
        }
    }
}