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

        // TODO : Refactor with IsOnGround in FixedUpdate
        if (Input.GetKeyDown(KeyCode.Space))
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
        if (_hasJumpInput)
        {
            _rigidbody.AddForce(_jumpForce * Vector2.up, ForceMode2D.Impulse);
            _hasJumpInput = false;
        }

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
}
