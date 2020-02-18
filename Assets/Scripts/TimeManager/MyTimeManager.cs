using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimeManager : MonoBehaviour
{
    public static MyTimeManager Instance { get; private set; }

    [SerializeField] private AnimationCurve _slowCurve;
    [SerializeField] private float _heroDeadTime = 5f;
    [SerializeField] private float _heroDeadScale = 0.1f;
    [SerializeField] private float _enemyDeadTime = 0.02f;
    [SerializeField] private float _enemyDeadScale = 0.1f;

    private bool _isSlow = false;
    private float _nextRecoverTime = 0f;
    private float _startSlowTime = 0f;
    private float _targetScale = 1f;

    private void Awake()
    {
        Instance = this;
    }

    public void StartHeroDead()
    {
        _isSlow = true;
        _startSlowTime = Time.realtimeSinceStartup;
        _targetScale = _heroDeadScale;
        _nextRecoverTime = _heroDeadTime + _startSlowTime;
    }


    public void StartEnemyDead()
    {
        _isSlow = true;
        _startSlowTime = Time.realtimeSinceStartup;
        _targetScale = _enemyDeadScale;
        _nextRecoverTime = _enemyDeadTime + _startSlowTime;
    }

    public void FixedUpdate()
    {
        if (_isSlow)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, _targetScale, _slowCurve.Evaluate(Time.realtimeSinceStartup - _startSlowTime));
            if (_nextRecoverTime <= Time.realtimeSinceStartup)
            {
                Time.timeScale = 1f;
                _isSlow = false;

            }
        }

    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }
}
