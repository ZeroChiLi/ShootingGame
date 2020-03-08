using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoGhostEffect : GoEffectBase
{
    private class MeshContext
    {
        public Mesh _mesh;
        private CombineInstance[] _arrCombine;
        private MeshFilter[] _arrMeshFilter;
        public float _endTime;

        // 初始化网格信息
        public MeshContext(ref MeshFilter[] arrMeshFilter)
        {
            CombineInstance[] combineInstances = new CombineInstance[arrMeshFilter.Length]; //新建一个合并组，长度与 meshfilters一致
            for (int i = 0; i < arrMeshFilter.Length; i++)                                  //遍历
            {
                combineInstances[i].mesh = arrMeshFilter[i].sharedMesh;                   //将共享mesh，赋值
                //combineInstances[i].transform = arrMeshFilter[i].transform.localToWorldMatrix; //本地坐标转矩阵，赋值
            }
            _mesh = new Mesh();                                  //声明一个新网格对象
            //_mesh.CombineMeshes(combineInstances);
            _arrCombine = combineInstances;
            _arrMeshFilter = arrMeshFilter;
        }

        // 更新网格位置
        public void Update(Transform target, float endTime)
        {
            if (_mesh == null)
                return;

            _endTime = endTime;

            for (int i = 0; i < _arrCombine.Length; i++)
                _arrCombine[i].transform = _arrMeshFilter[i].transform.localToWorldMatrix;

            _mesh.CombineMeshes(_arrCombine);
        }

        // 绘制网格
        public void Draw(Material material, float curTime)
        {
            if (_endTime < curTime)
                return;

            Graphics.DrawMesh(_mesh, Matrix4x4.identity, material, 0);  //合并网格的时候已经转出世界坐标了，所有直接用单位矩阵
        }
    }

    public Material _Material;
    public int _SampleCount = 5;
    public float _SampleInterval = 0.1f;
    public float _GhostLifeTime = 1f;

    private List<MeshContext> _contextList = new List<MeshContext>();
    private MeshFilter[] _arrMeshFilter;
    private int _curSampleIndex = -1;
    private float _nextSampleTime;

    public override void Init(GameObject go, object context)
    {
        base.Init(go, context);
        _arrMeshFilter = _SrcGo.GetComponentsInChildren<MeshFilter>();
    }

    public override void Play()
    {
        base.Play();
        _curSampleIndex = -1;
    }

    public override void Restore()
    {
        _contextList.Clear();
    }

    protected override void OnUpdate(float playTime)
    {
        int contextCount = _contextList.Count;
        var curTime = Time.time;
        if (_nextSampleTime < Time.time)
        {
            _nextSampleTime += _SampleInterval;
            _curSampleIndex = (_curSampleIndex + 1) % _SampleCount;

            if (contextCount <= _curSampleIndex)
            {
                var context = new MeshContext(ref _arrMeshFilter);
                _contextList.Add(context);
            }
            _contextList[_curSampleIndex].Update(transform, _GhostLifeTime + curTime);
        }
        for (int i = 0; i < contextCount; i++)
        {
            _contextList[i].Draw(_Material, curTime);
        }
    }

    public override bool IsValid()
    {
        return _Material != null;
    }
}
