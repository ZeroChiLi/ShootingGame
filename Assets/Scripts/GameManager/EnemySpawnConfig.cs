using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ShootGame/EnemeySpawnConfig")]
public class EnemySpawnConfig : ScriptableObject
{
    public GameObject Prefab;
    public Vector3 SpawnPos;
    public MoveDirection DefualtMoveDir;
    public int TotalCount = 10;
    public float DelaySpawnTime = 3;
    public float SpawnIntervalTime = 1;


}
