using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float duration = 5;
    private float _deleteTime;

    void Start()
    {
        _deleteTime = Time.time + duration;
    }

    // Update is called once per frame
    void Update()
    {
        if (_deleteTime < Time.time)
        {
            Object.Destroy(gameObject);
        }
    }
}
