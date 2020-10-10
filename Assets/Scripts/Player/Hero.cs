using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : PlayerBase
{
    public Collider hurtCollider;
    public bool isAlwaysInvincible = true;    // 是否无敌
    public float hurtInvincibleDuration = 1f;   // 受击后无敌时间 
    private bool _isHurtInvincible = false;      // 正在受击无敌
    private float _nextNotInvincibleTime = 0f;   // 下一次解除无敌时间

    protected new void Awake()
    {
        base.Awake();
    }

    protected new void Start()
    {
        GameManager.Instance.Hero = this;
        base.Start();
    }

    protected new void Update()
    {
        base.Update();
        if (_isHurtInvincible && _nextNotInvincibleTime < Time.time)
        {
            hurtCollider.gameObject.layer = LayerMask.NameToLayer("Player");
            _isHurtInvincible = false;
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (!isAlwaysInvincible && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GetHurt(new HurtContext(10, 10, collision.transform.position, (collision.transform.position - transform.position).normalized));
            //OnDead(collision.transform.position, 10);
            _isHurtInvincible = true;
            _nextNotInvincibleTime = Time.time + hurtInvincibleDuration;
            hurtCollider.gameObject.layer = LayerMask.NameToLayer("Invincible");
        }
    }

    protected override void OnDead(HurtContext context)
    {
        base.OnDead(context);
        MyTimeManager.Instance.StartHeroDead();

        gameObject.SetActive(false);
    }

}
