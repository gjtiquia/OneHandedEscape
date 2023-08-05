using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Run Settings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _decceleration;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce;

    [Header("Fall Settings")]
    [SerializeField] private float _fallGravityMultiplier;

    [Header("OnGround OverlapBox")]
    [SerializeField] private Vector2 _onGroundOverlapBoxPointOffset;
    [SerializeField] private Vector2 _onGroundOverlapBoxSize;
    [SerializeField] private LayerMask _onGroundOverlapLayerMask;

    private Rigidbody2D _rigidbody;
    private float _originalGravityScale;
    private int _moveInput = 0;
    private bool _hasJumpInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _originalGravityScale = _rigidbody.gravityScale;
    }

    private void Update()
    {
        // TODO : Refactor with Command Pattern and a Queue (Probably first filter the commands, like multiple left/right/jump condense, then use the commands)

        if (Input.GetKey(KeyCode.D))
        {
            _moveInput = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _moveInput = -1;
        }
        else
        {
            _moveInput = 0;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            _hasJumpInput = true;
        }
    }

    private void FixedUpdate()
    {
        // Run
        float targetSpeed = _moveInput * _moveSpeed;
        float speedDiff = targetSpeed - _rigidbody.velocity.x;
        float acceleration = Mathf.Abs(targetSpeed) > 0.01f ? _acceleration : _decceleration;
        float force = speedDiff * acceleration;

        _rigidbody.AddForce(force * Vector2.right);

        // Jump
        if (_hasJumpInput && IsOnGround())
        {
            _rigidbody.AddForce(_jumpForce * Vector2.up, ForceMode2D.Impulse);
        }
        _hasJumpInput = false;

        // Fall Gravity Multiplier
        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.gravityScale = _originalGravityScale * _fallGravityMultiplier;
        }
        else
        {
            _rigidbody.gravityScale = _originalGravityScale;
        }
    }

    private bool IsOnGround()
    {
        Collider2D collider = Physics2D.OverlapBox((Vector2)transform.position + _onGroundOverlapBoxPointOffset, _onGroundOverlapBoxSize, angle: 0, _onGroundOverlapLayerMask);

        bool isOnGround = collider;
        Debug.Log(isOnGround);
        Debug.Log(collider != null);
        return isOnGround;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)_onGroundOverlapBoxPointOffset, _onGroundOverlapBoxSize);
    }
}
