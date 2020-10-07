using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBullet : BulletBase
{
    [SerializeField] private GameObject _bombEffect;

    public override void Fire(Vector3 vec3)
    {
        _fireTime = Time.time;
        _destoryTime = _fireTime + _lifeTime;

        if (_bombEffect)
        {
            var _temGo = Object.Instantiate(_bombEffect);
            _temGo.transform.SetParent(GameManager.Instance.EffectGoRoot);
            _temGo.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        OnHit(collider, transform.position, collider.transform.position - transform.position,transform.position - collider.transform.position);
    }
}
