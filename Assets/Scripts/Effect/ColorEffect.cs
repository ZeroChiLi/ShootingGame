using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorEffect : PostEffectsBase
{
    [SerializeField] private AnimationCurve _alphaCurve;
    [Range(0, 1)]
    [SerializeField] private float _width = 0.05f;
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private float _duration = 0.5f;


    // 所有选择物体的网格
    private MeshFilter[] _meshFilters;
    private float _startTime = 0;
    private float _durationTime = 0;
    private float _endTime = 0;

    private void Update()
    {
        if (TargetMaterial != null && _endTime > Time.time)
        {
            _color.a = _alphaCurve.Evaluate((Time.time - _startTime) / (_durationTime));
            TargetMaterial.SetColor("_Color", _color);
            TargetMaterial.SetFloat("_Width", _width);

            if (_meshFilters == null)
                _meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
            for (int j = 0; j < _meshFilters.Length; j++)
                Graphics.DrawMesh(_meshFilters[j].sharedMesh, _meshFilters[j].transform.localToWorldMatrix, TargetMaterial, 0);   // 对选中物体再次渲染。

        }
    }

    public void Play()
    {
        Play(_color, _duration);
    }

    public void Play(Color color, float duration)
    {
        _startTime = Time.time;
        _durationTime = duration;
        _endTime = _startTime + duration;
    }
}