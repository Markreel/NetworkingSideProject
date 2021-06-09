using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 followOffset;

    private Transform target;

    private void Update()
    {
        if(target == null) { return; }
        transform.position = target.position + followOffset;
    }

    public void Follow(Transform _target)
    {
        target = _target;
    }
}
