using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _lerpSpeed;

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, _lerpSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, _target.rotation, _lerpSpeed * Time.deltaTime);
    }
}
