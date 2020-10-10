using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour, IBullet
{
    [SerializeField] protected BulletConfig _config;
    [SerializeField] protected bool _isAutoDestory = true;
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected float _lifeTime = 3f;
    protected float _fireTime = 0f;
    protected float _destoryTime = 0f;
    protected MeshRenderer _meshRenderer;
    public BulletConfig Config { get => _config; }
    private bool _isFire = false;
    private bool _isOnCollision = false;
    private bool _isOnHit = false;

    public float srcRotateY = 0;

    virtual protected void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    virtual public void Fire(Vector3 vec3)
    {
        _isFire = true;
        _rigidbody.AddForce(vec3);
        _fireTime = Time.time;
        _destoryTime = _fireTime + _lifeTime;
        srcRotateY = transform.rotation.eulerAngles.y;

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
        OnUpdate();
    }

    virtual protected void OnUpdate()
    {
    }

    private GameObject _temGo;
    virtual protected bool OnHit(Collider collider, Vector3 point, Vector3 normal, Vector3 fromDir)
    {
        _isOnHit = true;
        bool hitBulletReceiver = false;
        if (_config.HitEffect)
        {
            _temGo = Object.Instantiate(_config.HitEffect);
            _temGo.transform.SetParent(GameManager.Instance.EffectGoRoot);
            _temGo.transform.position = point;
        }
        var receiver = collider.GetComponentInParent<BulletReceiver>();
        if (receiver != null)
        {
            receiver.OnHit(this, point, normal);
            hitBulletReceiver = true;
        }
        CameraController.Instance.PlayShake(_config.ImpactPower / 200, _config.ImpactPower / 20);
        if (_isAutoDestory)
        {
            Destroy(gameObject);
        }
        return hitBulletReceiver;
    }

}
