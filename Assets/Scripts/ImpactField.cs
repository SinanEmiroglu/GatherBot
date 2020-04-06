using System;
using UnityEngine;

public class ImpactField : MonoBehaviour
{
    Transform _transform;
    Collider _collider;
    private void Awake()
    {
        _transform = transform;
        _collider = GetComponent<Collider>();
    }

    public bool IsImpacted(Transform obj) 
    {
        var distance = Vector3.SqrMagnitude(obj.position - transform.position);
        var totalLength = _transform.localScale.x + obj.localScale.x;

        return distance <= totalLength * totalLength;
    }
}