using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ShootGame/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    public BulletConfig BulletConfig;   // 子弹配置
    public GameObject FireEffect;   // 发射特效
    public float BulletSpeed;       // 子弹速度
    public float Accuracy;          // 精准度
    public float Interval;          // 发射间隔
    public float Recoil;            // 后坐力
    public uint BulletCount;        // 每次发射子弹数量 
}
