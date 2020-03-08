using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PlayerColdDownSkillBase : IPlayerSkill
{
    public float coldDownTime { get; protected set; }
    public bool isCanPlay { get; protected set; }
    protected float _nextFireTime;
    private float _curTime;

    virtual public bool Init(object context)
    {
        this.coldDownTime = coldDownTime;
        isCanPlay = true;
        return true;
    }

    virtual public void Update(float curTime)
    {
        _curTime = curTime;
        if (_nextFireTime >= _curTime)
        {
            isCanPlay = false;
            return;
        }

        isCanPlay = true;
    }

    virtual public bool ReadyToPlay()
    {
        return isCanPlay;
    }

    virtual public bool Play()
    {
        _nextFireTime = _curTime + coldDownTime;
        return true;
    }
}
