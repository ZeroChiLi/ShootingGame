using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadModel : MonoBehaviour
{
    [SerializeField] protected Rigidbody _cap;
    [SerializeField] protected Rigidbody _body;
    
    public void Play(float power, Vector3 hurtPos,float radius)
    {
        if (_body)
            _body.AddExplosionForce(power, hurtPos, radius);
        if (_cap)
            _cap.AddExplosionForce(power, hurtPos, radius);
    }

}
