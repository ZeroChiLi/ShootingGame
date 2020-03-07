using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OcclusionHighLightEffect : MonoBehaviour
{
    public Material _material;
    public MeshRenderer[] _ArrMeshRender;
    public Color _color = Color.blue;
    public float _amount = 2;

    private void Awake()
    {
        if (_ArrMeshRender == null || _ArrMeshRender.Length == 0)
        {
            _ArrMeshRender = GetComponentsInChildren<MeshRenderer>();
        }
    }

    public bool IsValid()
    {
        return _ArrMeshRender != null && _material != null;
    }

    protected void Start()
    {
        if (!IsValid())
        {
            return;
        }
        var mat = GameObject.Instantiate(_material);
        mat.SetColor("_Color", _color);
        mat.SetFloat("_Amount", _amount);
        for (int i = 0; i < _ArrMeshRender.Length; i++)
        {
            var srcMats = _ArrMeshRender[i].materials;
            List<Material> mats = new List<Material>(srcMats);
            mats.Add(mat);
            //mats.AddRange(srcMats);
            _ArrMeshRender[i].materials = mats.ToArray();
        }
    }

}
