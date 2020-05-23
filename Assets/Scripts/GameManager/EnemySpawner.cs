using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner
{
    private EnemySpawnConfig _config;
    private float _startTime;
    private float _spawnTime;
    private uint _spawnCount = 0;
    private List<Enemy> _enemyList = new List<Enemy>();
    private Transform _root;

    public EnemySpawner(EnemySpawnConfig config,Transform root)
    {
        _config = config;
        _root = root;
    }

    public void Start(float curTime)
    {
        Stop();
        _startTime = curTime;
        _spawnTime = _config.DelaySpawnTime + curTime;
    }

    public void Stop()
    {
        int count = _enemyList.Count;
        for (int i = 0; i < count; i++)
            Object.Destroy(_enemyList[i].gameObject);
        _enemyList.Clear();
        _spawnCount = 0;
    }

    public void Update(float curTime)
    {
        if (_spawnCount >= _config.TotalCount)
        {
            return;
        }
        if (_spawnTime <= curTime)
        {
            SpawnEnmey();
            _spawnTime = _spawnTime + _config.SpawnIntervalTime;
            ++_spawnCount;
        }
    }

    private void SpawnEnmey()
    {
        var go = Object.Instantiate(_config.Prefab);
        go.name = $"Enemy_{_spawnCount}";
        go.transform.SetParent(_root);
        go.transform.position = _config.SpawnPos;
        var enemy = go.GetComponent<Enemy>();
        enemy.SetMoveDir(_config.DefualtMoveDir);

        _enemyList.Add(enemy);
    }

}
