using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0, 3, -10);
   
    void FixedUpdate()
    {
        if (_target != null)
        {
            transform.position = _target.position + _offset;
        }
    }
    
}
