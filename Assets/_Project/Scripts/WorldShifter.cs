using UnityEngine;
using UnityEngine.Events;

namespace Project
{
    public class WorldShifter : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _shiftAmount;
        [SerializeField] private Transform _shiftTransform;

        [Header("Events")]
        [SerializeField] private UnityEvent _onWorldShifted;

        public void ShiftLeft()
        {
            Vector3 currentPosition = _shiftTransform.position;
            currentPosition.x += _shiftAmount;
            _shiftTransform.position = currentPosition;

            AfterShifting();
        }

        public void ShiftRight()
        {
            Vector3 currentPosition = _shiftTransform.position;
            currentPosition.x -= _shiftAmount;
            _shiftTransform.position = currentPosition;

            AfterShifting();
        }

        public void ShiftDown()
        {
            Vector3 currentPosition = _shiftTransform.position;
            currentPosition.y += _shiftAmount;
            _shiftTransform.position = currentPosition;

            AfterShifting();
        }

        public void ShiftUp()
        {
            Vector3 currentPosition = _shiftTransform.position;
            currentPosition.y -= _shiftAmount;
            _shiftTransform.position = currentPosition;

            AfterShifting();
        }

        private void AfterShifting()
        {
            _onWorldShifted?.Invoke();
        }
    }
}
