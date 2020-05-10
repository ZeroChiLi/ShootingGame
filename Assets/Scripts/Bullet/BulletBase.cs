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
    private Vector3 _lastPosition;
    private bool _isFire = false;

    public float srcRotateY = 0;

    virtual public void Fire(Vector3 vec3)
    {
        _isFire = true;
        _rigidbody.AddForce(vec3);
        _fireTime = Time.time;
        _destoryTime = _fireTime + _lifeTime;
        srcRotateY = transform.rotation.eulerAngles.y;
        _lastPosition = transform.position;

    }

    public void Update()
    {
        if (_isFire == false)
        {
            return;
        }
        if (_destoryTime <= Time.time)
        {
            Destroy(gameObject);
        }
        transform.rotation = Quaternion.Euler(0, srcRotateY - transform.forward.z * _rigidbody.velocity.z, _rigidbody.velocity.y);
        RaycastHit hit;
        var curPosition = transform.position;
        var shootDir = curPosition - _lastPosition;
        Debug.DrawRay(_lastPosition, shootDir, Color.Lerp(Color.green, Color.red, Time.time - _fireTime), shootDir.magnitude * 1.1f);
        if (Physics.Raycast(_lastPosition, shootDir, out hit, shootDir.magnitude * 1.1f, ~(1 << LayerMask.NameToLayer("Bullet"))))
        {
            OnHit(hit);
        }
        _lastPosition = curPosition;
    }

    private GameObject _temGo;

    virtual protected void OnHit(RaycastHit hit)
    {
        if (_config.HitEffect)
        {
            _temGo = Object.Instantiate(_config.HitEffect);
            _temGo.transform.SetParent(GameManager.Instance.EffectGoRoot);
            _temGo.transform.position = hit.point;
        }
        CameraController.Instance.PlayShake(_config.ImpactRange / 200, _config.ImpactRange / 20);
        Destroy(gameObject);
    }

}
