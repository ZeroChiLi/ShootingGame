using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0, 3, -10);
    [SerializeField] private float _smoothTime = 5f;
    [SerializeField] private float _dirOffest = 5f;
    [SerializeField] private float _turnAngleRange = 15f;
    [SerializeField] private float _turnAngleSpeed = 10f;

    private Vector3 _targetPos;
    private Vector3 _targetAngle;
    private Vector3 _posSmoothVel;
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
            transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _posSmoothVel, _smoothTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_targetAngle), Time.deltaTime * _turnAngleSpeed);

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
        float angle = 0;
        switch (direction)
        {
            case MoveDirection.Left:
                _curOffest = -_dirOffest;
                angle = -_turnAngleRange;
                break;
            case MoveDirection.Right:
                _curOffest = _dirOffest;
                angle = _turnAngleRange;
                break;
            default:
                _curOffest = 0f;
                angle = 0f;
                break;
        }
        _targetAngle = new Vector3(0, angle, 0);
    }

    public void PlayShake(float power, float duration)
    {
        _shakePower = power;
        _stopShakeTime = Time.time + duration;
    }
}
