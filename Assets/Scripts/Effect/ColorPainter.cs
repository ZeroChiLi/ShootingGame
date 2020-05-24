using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPainter : MonoBehaviour
{
    public MeshFilter meshFilter;
    public float innerRadius;
    public float outerRadius;
    public Color color;

    private Color[] colors;
    private Vector3[] vertices;
    private Mesh mesh;

    private void Awake()
    {
        mesh = meshFilter.mesh;
        vertices = mesh.vertices;
        colors = new Color[vertices.Length];
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                ApplyPaint(hitInfo.point, innerRadius, outerRadius, color);
            }
        }
    }

    public void ApplyPaint(Vector3 position, float innerRadius, float outerRadius, Color color)
    {
        //将坐标转化为本地坐标
        Vector3 center = transform.InverseTransformPoint(position);
        //将外半径转化为本地坐标
        float outerR = transform.InverseTransformVector(outerRadius * Vector3.right).magnitude;
        //将内半径转化为本地坐标
        float innerR = innerRadius * outerR / outerRadius;
        //计算内外半径平方待比较用
        float innerRsqr = innerR * innerR;
        float outerRsqr = outerR * outerR;
        //计算差值系数
        float tFactor = 1f / (outerR - innerR);
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 delta = vertices[i] - center;
            float dsqr = delta.sqrMagnitude;
            //拿到距离的平方
            //如果大于外半径则不理会
            if (dsqr > outerRsqr) continue;
            float a = colors[i].a;
            colors[i] = new Color(color.r, color.g, color.b, colors[i].a);
            //小于内半径则把顶点颜色透明度设置为不透明，触发shader中的比较
            if (dsqr < innerRsqr)
            {
                colors[i].a = 1;
            }
            else
            {
                //拿到距离
                float d = Mathf.Sqrt(dsqr);
                float t = 1f - (d - innerR) * tFactor;
                if (t >= colors[i].a)
                {
                    colors[i].a = t;
                }
            }
        }
        mesh.colors = colors;
    }
}