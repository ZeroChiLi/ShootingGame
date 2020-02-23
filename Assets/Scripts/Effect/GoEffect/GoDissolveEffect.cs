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
    public AnimationCurve _Value = AnimationCurve.Linear(0, 0, 1, 1);

    // 溶解现场
    private DissolveContext _DissolveContext = new DissolveContext();

    public override bool IsValid()
    {
        return _Material != null;
    }

    public override void Init(GameObject go, object context)
    {
        base.Init(go, context);
        _DissolveContext.Init(_SrcGo.GetComponentsInChildren<MeshRenderer>(), _Material);
    }

    public override void Play()
    {
        base.Play();
        _DissolveContext.TurnDissolve();
    }

    public override void Restore()
    {
        _DissolveContext.SetDissolveMatFloat("_DissolveThreshold", 0f);
        _DissolveContext.TurnOriginal();
    }

    protected override void OnUpdate(float playTime)
    {

        float value = _Value.Evaluate(playTime / _Duration);
        _DissolveContext.SetDissolveMatFloat("_DissolveThreshold", value);
    }

}
