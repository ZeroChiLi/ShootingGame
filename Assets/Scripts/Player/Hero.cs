using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : PlayerBase
{
    public bool isInvincible = true;    // 是否无敌

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
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (!isInvincible && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GetHurt(new HurtContext(10, 10, collision.transform.position, (collision.transform.position - transform.position).normalized));
            //OnDead(collision.transform.position, 10);
        }
    }

    protected override void OnDead(HurtContext context)
    {
        base.OnDead(context);
        MyTimeManager.Instance.StartHeroDead();

    }

}
