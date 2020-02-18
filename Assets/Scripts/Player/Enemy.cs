using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PlayerBase
{
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

    private BulletBase _temBullet;
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            _temBullet = collision.gameObject.GetComponent<BulletBase>();

            GetHurt(_temBullet.Config.AttactHurt, collision.transform.position, _temBullet.Config.ImpactRange);
        }
    }

    protected override void OnDead(Vector3 hurtSrc, float range)
    {
        base.OnDead(hurtSrc, range);

        MyTimeManager.Instance.StartEnemyDead();
        if (_bombPrefab && Random.value <= _bombProbaility)
        {
            var _temGo = Object.Instantiate(_bombPrefab);
            _temGo.transform.SetParent(GameManager.Instance.BulletGoRoot);
            _temGo.transform.position = transform.position;
            _temGo.GetComponent<BulletBase>().Fire(transform.position);
        }
        
    }
}
