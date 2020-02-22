using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoDissolveEffect : GoEffectBase
{//消融记录的现场
    public class DissolveContext
    {
        private class MaterialsPair
        {
            public Material[] First;
            public Material[] Second;
        }

        //渲染对象网格
        private MeshRenderer[] _ArrMesh;

        //渲染对象的材质
        private Dictionary<MeshRenderer, MaterialsPair> _MeshMaterialDic = new Dictionary<MeshRenderer, MaterialsPair>();

        /// <summary>
        /// 清除现场
        /// </summary>
        private void Clear()
        {
            _ArrMesh = null;
            _MeshMaterialDic.Clear();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="skins">对象所有需要消融的网格</param>
        /// <param name="dissolve">消融材质</param>
        public void Init(MeshRenderer[] skins, Material dissolve)
        {
            Clear();
            _ArrMesh = skins;
            int nCount = _ArrMesh.Length;
            for (int i = 0; i < nCount; ++i)
            {
                var skin = _ArrMesh[i];
                MaterialsPair materialsPair = new MaterialsPair();
                materialsPair.First = skin.materials;

                var mats = skin.materials;
                int matCount = mats.Length;
                List<Material> dissolveMatList = new List<Material>();
                for (int j = 0; j < matCount; j++)
                {
                    Material disMat = UnityEngine.Object.Instantiate(dissolve);
                    disMat.mainTexture = mats[j].mainTexture;
                    dissolveMatList.Add(disMat);
                }
                materialsPair.Second = dissolveMatList.ToArray();
                _MeshMaterialDic.Add(skin, materialsPair);
            }

        }

        /// <summary>
        /// 设置消融参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetDissolveMatFloat(string key, float value)
        {
            int nLen = _ArrMesh.Length;
            for (int i = 0; i < nLen; ++i)
            {
                var dissMats = _MeshMaterialDic[_ArrMesh[i]].Second;
                int matCount = dissMats.Length;
                for (int j = 0; j < matCount; j++)
                {
                    dissMats[j].SetFloat(key, value);
                }
            }
        }

        /// <summary>
        /// 切换到消融
        /// </summary>
        public void TurnDissolve()
        {
            TurnMat(false);
        }

        /// <summary>
        /// 切换到原始
        /// </summary>
        public void TurnOriginal()
        {
            TurnMat(true);
        }

        /// <summary>
        /// 切换材质
        /// </summary>
        /// <param name="isOriginal">是否原始，否则就是消融</param>
        private void TurnMat(bool isOriginal)
        {
            int nLen = _ArrMesh.Length;
            for (int i = 0; i < nLen; ++i)
            {
                var matPair = _MeshMaterialDic[_ArrMesh[i]];
                _ArrMesh[i].materials = isOriginal ? matPair.First : matPair.Second;
            }
        }

    }

    public Material _Material;
    public float _Duration = 3.0f;
    public float _Delay = 1;
    public bool _PlayAtAwake = true;
    public bool _RevertAtStop = true;
    public AnimationCurve _Value = AnimationCurve.Linear(0, 0, 1, 1);
    public UnityEvent _FinishedCallback;

    private float _StartDissolveTime;  //开始特效时间
    private bool _isRuning = false;         //是否在运行
                                            // 溶解现场
    private DissolveContext _DissolveContext = new DissolveContext();

    public override void Init(GameObject go,object context)
    {
        base.Init(go, context);
        _FinishedCallback = (UnityEvent)context;
    }

    public override void Play()
    {
        _isRuning = true;
        _StartDissolveTime = Time.time;
        _DissolveContext.TurnDissolve();
    }

    public override void Restore()
    {
        _DissolveContext.TurnOriginal();
    }

    public override void Stop()
    {
        _isRuning = false;
        if (_RevertAtStop)
        {
            _DissolveContext.SetDissolveMatFloat("_DissolveThreshold", 0f);
            _DissolveContext.TurnOriginal();
        }
    }
    
    void Start()
    {
        _DissolveContext.Init(_SrcGo.GetComponentsInChildren<MeshRenderer>(), _Material);
        if (_PlayAtAwake)
        {
            Init(gameObject, _FinishedCallback);
            Play();
        }
    }
    
    void Update()
    {
        if (!_isRuning)
            return;

        float curTime = Time.time;
        if (_StartDissolveTime + _Delay + _Duration < curTime)
        {
            if (_FinishedCallback != null)
                _FinishedCallback.Invoke();
            Stop();
            return;
        }
        if (_StartDissolveTime + _Delay > curTime)
        {
            return;
        }

        float value = _Value.Evaluate((curTime - _StartDissolveTime - _Delay) / _Duration);
        _DissolveContext.SetDissolveMatFloat("_DissolveThreshold", value);


    }
}
