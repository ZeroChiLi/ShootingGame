using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PlayerBase
{
    [SerializeField] private BulletReceiver _bulletReceiver;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private float _bombProbaility = 0.1f;  // 自爆概率

    private EnemyController _enemyController;


    public void SetMoveDir(MoveDirection dir)
    {
        _enemyController.SetMoveDir(dir);
    }

    protected new void Awake()
    {
        base.Awake();
        _enemyController = GetComponent<EnemyController>();
        if (_bulletReceiver != null)
            _bulletReceiver.Bind(OnHitBullet);
    }

    protected new void Start()
    {
        base.Start();
        _enemyController.StartMove();
    }

    protected new void Update()
    {
        base.Update();
    }

    public void OnHitBullet(BulletBase bullet, Vector3 hit, Vector3 normal)
    {
        GetHurt(new HurtContext(bullet.Config.AttactHurt, bullet.Config.ImpactRange, hit, normal));
    }

    protected override void OnDead(HurtContext context)
    {
        MyTimeManager.Instance.StartEnemyDead();
        if (_bombPrefab && Random.value <= _bombProbaility)
        {
            var _temGo = Object.Instantiate(_bombPrefab);
            _temGo.transform.SetParent(GameManager.Instance.BulletGoRoot);
            _temGo.transform.position = transform.position;
            _temGo.GetComponent<BulletBase>().Fire(transform.position);
        }

        base.OnDead(context);

    }
}
