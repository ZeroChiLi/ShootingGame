using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ShootGame/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public uint MaxHP = 10;
    public float MoveSpeed = 0.3f;
    public float TurnSpeed = 1000f;
    public float JumpSpeed = 300f;
    public float SprintSpeed = 1500f;
    public float SprintColdDownTime = 0.5f;
    public float SlowScaleWhenDeadTime = 0.1f;      // 死亡瞬间的减速时间缩放
    public float SlowDurationWhenDeadTime = 3;      // 死亡瞬间的持续时间

}
