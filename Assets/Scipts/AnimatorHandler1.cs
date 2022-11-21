using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler1 : MonoBehaviour
{
    private const string HorizontalParameter = "Horizontal";
    private const string VerticalParameter = "Vertical";
    private const string IsMovingParameter = "IsMoving";
    private const string RollTrigger = "Roll";

    [SerializeField] private Transform _currentTarget;
    [SerializeField] private float _rollDistance;
    [SerializeField] private float _rollAnimationDuration;

    [SerializeField] private Transform _body;
    [SerializeField] private Transform _rotation;
    [SerializeField] private float _rollSpeed;

    private bool _isLockedOnTarget = true;
    private bool _isAbleToRoll = true;
    private Movement _movement;
    private Rigidbody _rigidbody;
    private Animator _animator;

    private Vector3 _eulerLeft = new Vector3(0, -90, 0);
    private Vector3 _eulerRight = new Vector3(0, 90, 0);
    private Vector3 _eulerBack = new Vector3(0, 180, 0);

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _movement = GetComponent<Movement>();
        _rigidbody = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        _animator.SetFloat(HorizontalParameter, _movement.Horizontal);
        _animator.SetFloat(VerticalParameter, _movement.Vertical);
        _animator.SetBool(IsMovingParameter, _movement.IsMoving);
    }

    private void FixedUpdate()
    {
        if (_isLockedOnTarget)
        {
            Vector3 targetPosistion = new Vector3(_currentTarget.position.x, transform.position.y, _currentTarget.position.z);
            _rigidbody.rotation = Quaternion.LookRotation(targetPosistion - transform.position);
        }
    }

    public void Roll()
    {
        if (_isAbleToRoll)
            StartCoroutine(DoRoll());
    }

    protected Vector3 GetRollDirection()
    {
        Vector3 direction = Vector3.zero;

        if (Mathf.Abs(_movement.Horizontal) > Mathf.Abs(_movement.Vertical))
        {
            if (_movement.Horizontal > 0)
            {
                _body.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
                direction = Vector3.right;
            }
            else
            {
                _body.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
                direction = Vector3.left;
            }
        }
        else
        {
            if (_movement.Vertical < 0)
            {
                _body.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                direction = Vector3.back;
            }
            else
            {
                direction = Vector3.forward;
            }
        }
        Debug.Log(direction);
        return direction;
    }

    private IEnumerator DoRoll()
    {
        _isAbleToRoll = false;
        float elapsedTime = 0;
        Vector3 direction = GetRollDirection();
        _animator.SetTrigger(RollTrigger);
        _isLockedOnTarget = false;

        while (elapsedTime < _rollAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            Vector3 endPoint = transform.TransformDirection(direction * _rollDistance);
            transform.position = Vector3.MoveTowards(transform.position, endPoint, _rollSpeed * Time.deltaTime);
            yield return null;
        }
        _isLockedOnTarget = true;
        _body.localRotation = Quaternion.Euler(Vector3.zero);
        _isAbleToRoll = true;
        yield break;
    }
}
