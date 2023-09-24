using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityAssert = UnityEngine.Assertions.Assert;

namespace Project
{
    public class WorldShifter : MonoBehaviour
    {
        [SerializeField] private List<ShiftData> _shiftData;

        [Header("Events")]
        [SerializeField] private UnityEvent _onWorldShifted;

        private void OnValidate()
        {
            foreach (ShiftData data in _shiftData)
            {
                data.OnValidate();
            }
        }

        public void ShiftLeft()
        {
            foreach (ShiftData data in _shiftData)
            {
                data.ShiftLeft();
            }

            AfterShifting();
        }

        public void ShiftRight()
        {
            foreach (ShiftData data in _shiftData)
            {
                data.ShiftRight();
            }

            AfterShifting();
        }

        public void ShiftDown()
        {
            foreach (ShiftData data in _shiftData)
            {
                data.ShiftDown();
            }

            AfterShifting();
        }

        public void ShiftUp()
        {
            foreach (ShiftData data in _shiftData)
            {
                data.ShiftUp();
            }

            AfterShifting();
        }

        private void AfterShifting()
        {
            _onWorldShifted?.Invoke();
        }

        [System.Serializable]
        private class ShiftData
        {
            public Transform ShiftTransform;
            public float ShiftAmount;
            public bool ReverseDirection;

            private int _reverseMultiplier => ReverseDirection ? -1 : 1;

            public void OnValidate()
            {
                UnityAssert.IsNotNull(ShiftTransform);
            }

            public void ShiftLeft()
            {
                Vector3 currentPosition = ShiftTransform.position;
                currentPosition.x -= _reverseMultiplier * ShiftAmount;
                ShiftTransform.position = currentPosition;
            }

            public void ShiftRight()
            {
                Vector3 currentPosition = ShiftTransform.position;
                currentPosition.x += _reverseMultiplier * ShiftAmount;
                ShiftTransform.position = currentPosition;
            }

            public void ShiftDown()
            {
                Vector3 currentPosition = ShiftTransform.position;
                currentPosition.y -= _reverseMultiplier * ShiftAmount;
                ShiftTransform.position = currentPosition;
            }

            public void ShiftUp()
            {
                Vector3 currentPosition = ShiftTransform.position;
                currentPosition.y += _reverseMultiplier * ShiftAmount;
                ShiftTransform.position = currentPosition;
            }
        }
    }
}
