using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ShootGame/BulletConfig")]
public class BulletConfig : ScriptableObject
{
    public GameObject BulletPrefab; // 子弹预制体
    public GameObject ShellPrefab;  // 子弹壳预制体
    public GameObject HitEffect;    // 击中特效
    public int AttactHurt = 5;      // 伤害值
    public float ImpactRange = 10f; // 冲击范围
}
