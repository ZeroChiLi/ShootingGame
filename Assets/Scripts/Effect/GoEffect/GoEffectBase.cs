using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

abstract public class GoEffectBase : MonoBehaviour, IGoEffect
{
    [SerializeField] protected GameObject _SrcGo;
    [SerializeField] protected float _Duration = -1f;
    [SerializeField] protected float _Delay = 0f;
    [SerializeField] protected bool _PlayAtAwake = true;
    [SerializeField] protected bool _RestoreAtStop = true;
    [SerializeField] protected UnityEvent _FinishedCallback;

    protected float _StartTime;         //开始特效时间
    protected float _CurTime;           //当前时间
    protected bool _IsRunning = false;  //是否在运行

    /// <summary>
    /// 重置
    /// </summary>
    abstract public void Restore();

    /// <summary>
    /// 正在开始播放动画
    /// </summary>
    /// <param name="playTime">播放了多久，从0开始</param>
    abstract protected void OnUpdate(float playTime);

    /// <summary>
    /// 是否合法（能跑）
    /// </summary>
    /// <returns></returns>
    virtual public bool IsValid() { return true; }

    virtual public void Init(GameObject go, object context)
    {
        _SrcGo = go;
        _FinishedCallback = (UnityEvent)context;
    }

    virtual public void Play()
    {
        if (!IsValid())
            return;
        _IsRunning = true;
        _StartTime = Time.time;
    }

    virtual public void Stop()
    {
        _IsRunning = false;
        if (_RestoreAtStop)
        {
            Restore();
        }
    }

    virtual protected void Start()
    {
        if (_PlayAtAwake)
        {
            Init(gameObject, _FinishedCallback);
            Restore();
            Play();
        }
    }

    virtual protected void Update()
    {

        if (!_IsRunning)
            return;

        _CurTime = Time.time;
        if (_Duration > 0 && (_StartTime + _Delay + _Duration < _CurTime))
        {
            if (_FinishedCallback != null)
                _FinishedCallback.Invoke();
            Stop();
            return;
        }
        if (_StartTime + _Delay > _CurTime)
        {
            return;
        }

        OnUpdate(_CurTime - _StartTime - _Delay);
    }

}
