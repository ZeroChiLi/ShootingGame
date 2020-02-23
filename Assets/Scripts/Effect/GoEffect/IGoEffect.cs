using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoEffect
{
    void Init(GameObject go, object context);
    void Play();
    void Stop();
    void Restore();
    bool IsValid();
}
