using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOcclusionDissolveEffect : MonoBehaviour
{
    public Camera mainCamera;
    public Transform wallRoot;
    public Transform target;
    public Vector2 offset;

    private readonly Vector2 _center = new Vector2(0.5f, 0.5f);
    private const string _CenterX = "_CenterX";
    private const string _CenterY = "_CenterY";
    private List<Material> _wallMatList = new List<Material>();

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        if (wallRoot == null)
            wallRoot = transform;

        var arrRenderer = wallRoot.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < arrRenderer.Length; i++)
        {
            var mats = arrRenderer[i].materials;
            for (int j = 0; j < mats.Length; j++)
            {
                _wallMatList.Add(mats[j]);
            }
        }
    }

    void Update()
    {
        if (target == null || mainCamera == null || _wallMatList == null)
        {
            return;
        }
        SetCenter(mainCamera.WorldToViewportPoint(target.position));
    }

    private void SetCenter(Vector2 targetPos)
    {
        int count = _wallMatList.Count;
        for (int i = 0; i < count; i++)
        {
            _wallMatList[i].SetFloat(_CenterX, targetPos.x + offset.x);
            _wallMatList[i].SetFloat(_CenterY, targetPos.y + offset.y);
        }
    }
}
