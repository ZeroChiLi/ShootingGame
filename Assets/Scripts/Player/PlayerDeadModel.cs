using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadModel : MonoBehaviour
{
    [SerializeField] protected float _deadPower = 100f;
    [SerializeField] protected GameObject _hitEffect;
    [SerializeField] protected Rigidbody _cap;
    [SerializeField] protected Rigidbody _body;

    public void Play(LinkedList<HurtContext> hurtContexts)
    {
        var lastHurt = hurtContexts.First.Value;
        var power = lastHurt.hurtValue * _deadPower;
        var hurtPos = lastHurt.hitPoint;
        var radius = lastHurt.impactRange;
        //Debug.LogError($"死掉了 {gameObject.name} {power} {hurtPos} {radius} {normal}");
        if (_body)
        {
            _body.AddExplosionForce(power, hurtPos, radius);
            if (_hitEffect)
            {
                foreach (var item in hurtContexts)
                {
                    GameObject spawnedDecal = GameObject.Instantiate(_hitEffect, item.hitPoint, Quaternion.LookRotation(item.hitNormal));
                    spawnedDecal.transform.SetParent(_body.transform);
                }
            }
        }
        if (_cap)
            _cap.AddExplosionForce(power, hurtPos, radius);
    }

}
