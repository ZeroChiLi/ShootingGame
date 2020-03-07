using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoScaleEffect : GoEffectBase
{
    public float _BaseSize = 2f;
    public AnimationCurve _SizeCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public override void Restore()
    {
        _SrcGo.transform.localScale = _BaseSize * _SizeCurve.Evaluate(0) * Vector3.one;
    }

    protected override void OnUpdate(float playTime)
    {
        float size = _SizeCurve.Evaluate((playTime) / _Duration);
        _SrcGo.transform.localScale = _BaseSize * size * Vector3.one;

    }

}
