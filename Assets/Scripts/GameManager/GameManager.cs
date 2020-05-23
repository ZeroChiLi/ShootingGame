using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemySpawnConfig _enemySpawnConfig;
    [SerializeField] private GameObject _theEndUI;
    [SerializeField] private Transform _enemyGoRoot;
    [SerializeField] private Transform _bulletGoRoot;
    [SerializeField] private Transform _deadModelGoRoot;
    [SerializeField] private Transform _effectGoRoot;
    public Transform BulletGoRoot { get => _bulletGoRoot; }
    public Transform EffectGoRoot { get => _effectGoRoot; }
    public Transform DeadModelGoRoot { get => _deadModelGoRoot; }
    public Hero Hero { get; set; }

    private EnemySpawner _enemySpawner;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _enemySpawner = new EnemySpawner(_enemySpawnConfig, _enemyGoRoot);
    }

    private void Start()
    {
        _enemySpawner.Start(Time.time);
    }

    private void Update()
    {
        _enemySpawner.Update(Time.time);
        if (Hero && Hero.State == PlayerState.Dead)
        {
            _theEndUI.SetActive(true);
        }
    }

    public void ReStart()
    {
        var sceneName = SceneManager.GetActiveScene().name;//获取场景名称
        //index = SceneManager.GetActiveScene().buildIndex;//获取场景所在序号
        SceneManager.LoadScene(sceneName);//加载所需场景,SceneName为场景名
    }
}
