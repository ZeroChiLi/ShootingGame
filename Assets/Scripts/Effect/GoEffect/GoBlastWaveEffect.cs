using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoBlastWaveEffect : GoScaleEffect
{
    public MeshRenderer[] _ArrMeshRender;
    public AnimationCurve _AlphaCurve = AnimationCurve.Linear(0, 1, 1, 0);

    private void Awake()
    {
        if (_ArrMeshRender == null || _ArrMeshRender.Length == 0)
        {
            _ArrMeshRender = GetComponentsInChildren<MeshRenderer>(true);
        }
    }

    public override bool IsValid()
    {
        return _ArrMeshRender != null;
    }

    public override void Restore()
    {
        for (int i = 0; i < _ArrMeshRender.Length; i++)
        {
            _ArrMeshRender[i].material.SetFloat("_Alpha", _AlphaCurve.Evaluate(0));

        }
    }

    protected override void OnUpdate(float playTime)
    {
        base.OnUpdate(playTime);
        float alpha = _AlphaCurve.Evaluate((playTime) / _Duration);
        for (int i = 0; i < _ArrMeshRender.Length; i++)
        {
            _ArrMeshRender[i].material.SetFloat("_Alpha", alpha);

        }


    }

}
