using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody _rigidbody;
    private Vector3 _moveDirection;
    private float _horizontalInput;
    private float _verticalInput;
    private bool _isMoving;

    public bool IsMoving => _isMoving;
    public float Horizontal => _horizontalInput;
    public float Vertical => _verticalInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _horizontalInput = 0;
            _verticalInput = Vector3.forward.z;
            _isMoving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _horizontalInput = 0;
            _verticalInput = Vector3.back.z;
            _isMoving = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _verticalInput = 0;
            _horizontalInput = Vector3.left.x;
            _isMoving = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _verticalInput = 0;
            _horizontalInput = Vector3.right.x;
            _isMoving = true;
        }
        else
        {
            _horizontalInput = 0;
            _verticalInput = 0;
            _isMoving = false;
        }
        Move();           
    }

    private void Move()
    {
        _moveDirection = transform.forward * _verticalInput + transform.right * _horizontalInput;
        Vector3 velocity = _moveDirection * _speed;
        _rigidbody.velocity = velocity;
    }
}

