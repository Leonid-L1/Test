using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    private const string HorizontalParameter = "Horizontal";
    private const string VerticalParameter = "Vertical";
    private const string IsMovingParameter = "IsMoving";
    private const string RollTrigger = "Roll";

    [SerializeField] private AnimationCurve _rollCurveY;
    [SerializeField] private AnimationCurve _rollCurveZ;

    [SerializeField] private Transform _currentTarget;
    [SerializeField] private float _rollDistance;
    [SerializeField] private float _rollAnimationDuration;

    [SerializeField] private Transform _body;
    [SerializeField] private Transform _rotation;

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
                _body.localRotation = Quaternion.Euler(_eulerRight);
                direction = Vector3.right;
            }
            else
            {
                _body.localRotation = Quaternion.Euler(_eulerLeft);
                direction = Vector3.left;
            }
        }
        else
        {
            if (_movement.Vertical < 0)
            {
                _body.localRotation = Quaternion.Euler(_eulerBack);
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

    private Vector3 EvaluateCurve(Vector3 direction, float progress)
    {
        Vector3 evaluatedCurve = Vector3.zero;

        if (direction == Vector3.right)
        {
            evaluatedCurve = new Vector3(_rollCurveZ.Evaluate(progress), _rollCurveY.Evaluate(progress), 0);
        }
        else if (direction == Vector3.left)
        {
            evaluatedCurve = new Vector3(_rollCurveZ.Evaluate(progress) * -1, _rollCurveY.Evaluate(progress), 0);
        }
        else if (direction == Vector3.back)
        {
            evaluatedCurve = new Vector3(0, _rollCurveY.Evaluate(progress), _rollCurveZ.Evaluate(progress) * -1);
        }
        else
        {
            evaluatedCurve = new Vector3(0, _rollCurveY.Evaluate(progress), _rollCurveZ.Evaluate(progress));
        }

        return transform.TransformDirection(evaluatedCurve);
    }

    private IEnumerator DoRoll()
    {
        _isAbleToRoll = false;
        float elapsedTime = 0;
        float progress = 0;
        Vector3 direction = GetRollDirection();
        Vector3 startPosition = transform.localPosition;
        _animator.SetTrigger(RollTrigger);
        _isLockedOnTarget = false;

        while (progress < 1)
        {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / _rollAnimationDuration;

            transform.localPosition = startPosition + EvaluateCurve(direction, progress) * _rollDistance;
            yield return null;
        }
        _isLockedOnTarget = true;
        _body.localRotation = Quaternion.Euler(Vector3.zero);
        _isAbleToRoll = true;
        yield break;
    }
}
