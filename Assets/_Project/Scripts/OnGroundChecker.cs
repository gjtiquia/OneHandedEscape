using UnityEngine;

namespace Project
{
    public class OnGroundChecker : MonoBehaviour, IOnGroundChecker
    {
        [SerializeField] private Vector2 _onGroundOverlapBoxPointOffset;
        [SerializeField] private Vector2 _onGroundOverlapBoxSize;
        [SerializeField] private LayerMask _onGroundOverlapLayerMask;

        public bool IsOnGround()
        {
            return Physics2D.OverlapBox((Vector2)transform.position + _onGroundOverlapBoxPointOffset, _onGroundOverlapBoxSize, angle: 0, _onGroundOverlapLayerMask);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + (Vector3)_onGroundOverlapBoxPointOffset, _onGroundOverlapBoxSize);
        }
    }
}