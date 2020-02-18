using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour, IBullet
{
    [SerializeField] protected BulletConfig _config;
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected float _lifeTime = 3f;
    protected float _fireTime = 0f;
    protected float _destoryTime = 0f;
    public BulletConfig Config { get => _config; }

    virtual public void Fire(Vector3 vec3)
    {
        _rigidbody.AddForce(vec3);
        _fireTime = Time.time;
        _destoryTime = _fireTime + _lifeTime;

    }

    public void Update()
    {
        if (_destoryTime <= Time.time)
        {
            Destroy(gameObject);
        }
    }

    private GameObject _temGo;
    private void OnCollisionEnter(Collision collision)
    {
        if (_config.HitEffect)
        {
            _temGo = Object.Instantiate(_config.HitEffect);
            _temGo.transform.SetParent(GameManager.Instance.EffectGoRoot);
            _temGo.transform.position = transform.position;
        }
        CameraController.Instance.PlayShake(_config.ImpactRange / 200, _config.ImpactRange / 20);
        Destroy(gameObject);
    }

}
