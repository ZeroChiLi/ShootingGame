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
            GetHurt(10, collision.transform.position, 10);
            //OnDead(collision.transform.position, 10);
        }
    }

    protected override void OnDead(Vector3 hurtSrc, float range)
    {
        base.OnDead(hurtSrc, range);
        MyTimeManager.Instance.StartHeroDead();

    }

}
