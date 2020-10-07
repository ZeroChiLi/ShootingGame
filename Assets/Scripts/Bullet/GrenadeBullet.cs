using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBullet : FristBullet
{
    public BulletBase boomBullet;
    public float boomTime = 2f;
    public int hitTime = 4;
    private int curHitTime = 0;
    private bool isFireBoom =false;

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (_fireTime + boomTime < Time.time)
        {
            Boom();
        }
    }

    private void Boom()
    {
        if (isFireBoom)
            return;
        isFireBoom = true;
        if (boomBullet != null)
        {
            var _temGo = Object.Instantiate(boomBullet);
            _temGo.transform.SetParent(GameManager.Instance.BulletGoRoot);
            _temGo.transform.position = transform.position;
            _temGo.GetComponent<BulletBase>().Fire(Vector3.zero);
        }
        Destroy(gameObject);
    }

    protected override bool OnHit(Collider collider, Vector3 point, Vector3 normal, Vector3 fromDir)
    {
        var hitBulletReceiver = base.OnHit(collider, point, normal, fromDir);
        ++curHitTime;
        if (hitBulletReceiver || curHitTime >= hitTime)
        {
            Boom();
            return hitBulletReceiver;
        }
        transform.position = point;
        var reflectVec = GetReflectVec(fromDir, normal);
        _rigidbody.velocity = _rigidbody.velocity * 0f;
        _rigidbody.AddForce(reflectVec * _fireStartVec3.magnitude * ( 1 - curHitTime / hitTime));
        return hitBulletReceiver;
    }
    
    private Vector3 GetReflectVec(Vector3 from, Vector3 normal)
    {
        var nor = normal.normalized;
        return from - 2 * (Vector3.Dot(from, nor)) * nor;
    }
}
