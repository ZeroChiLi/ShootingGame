using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0, 3, -10);
    [SerializeField] private float _smoothTime = 5f;
    [SerializeField] private float _dirOffest = 5f;

    private Vector3 _targetPos;
    private Vector3 _smoothVel;
    private float _curOffest;
    private float _shakePower;
    private float _stopShakeTime;

    public static CameraController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    Vector2 _temVec2;
    void FixedUpdate()
    {
        if (_target)
        {
            _targetPos = _target.position + _offset;
            _targetPos.x += _curOffest;
            transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _smoothVel, _smoothTime);

            if (_stopShakeTime > Time.time)
            {
                _temVec2 = Random.insideUnitCircle * _shakePower;
                transform.position = transform.position + new Vector3(_temVec2.x, _temVec2.y, 0);
            }
        }
    }

    public void SetTarget(Transform target, MoveDirection direction = MoveDirection.None)
    {
        _target = target;
        switch (direction)
        {
            case MoveDirection.Left:
                _curOffest = -_dirOffest;
                break;
            case MoveDirection.Right:
                _curOffest = _dirOffest;
                break;
            default:
                _curOffest = 0f;
                break;
        }
    }

    public void PlayShake(float power, float duration)
    {
        _shakePower = power;
        _stopShakeTime = Time.time + duration;
    }
}
