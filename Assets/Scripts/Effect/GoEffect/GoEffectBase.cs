using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class GoEffectBase : MonoBehaviour, IGoEffect
{
    [SerializeField] protected GameObject _SrcGo;
    virtual public void Init(GameObject go, object context)
    {
        _SrcGo = go;
    }

    abstract public void Play();

    abstract public void Restore();

    abstract public void Stop();

}
