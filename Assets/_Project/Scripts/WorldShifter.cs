using UnityEngine;

namespace Project
{
    public class WorldShifter : MonoBehaviour
    {
        [SerializeField] private float _shiftAmount;
        [SerializeField] private Transform _shiftTransform;

        public void ShiftLeft()
        {
            Vector3 currentPosition = _shiftTransform.position;
            currentPosition.x += _shiftAmount;
            _shiftTransform.position = currentPosition;
        }

        public void ShiftRight()
        {
            Vector3 currentPosition = _shiftTransform.position;
            currentPosition.x -= _shiftAmount;
            _shiftTransform.position = currentPosition;
        }
    }
}
