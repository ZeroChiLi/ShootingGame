using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField] protected WeaponConfig _config;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected Transform _clipPoint;
    public WeaponConfig Config { get => _config; }
    private float _nextFireTime = 0f;

    protected void Start()
    {

    }

    private float _temTime;
    private GameObject _temGo;
    private BulletBase _temBullet;
    private Vector2 _temVec2;
    virtual public bool Fire()
    {
        _temTime = Time.time;
        if (_nextFireTime >= _temTime)
            return false;

        _nextFireTime = _temTime + _config.Interval;

        for (int i = 0; i < _config.BulletCount; i++)
        {
            _temGo = Object.Instantiate(_config.BulletConfig.BulletPrefab);
            _temGo.transform.SetParent(GameManager.Instance.BulletGoRoot);
            _temGo.transform.position = _firePoint.position;
            var rotateOffsetY = (Random.value - 0.5f) * 2 * _config.Accuracy;
            var rotateOffsetX = (Random.value - 0.5f) * 2 * _config.Accuracy;

            _temGo.transform.rotation = _firePoint.rotation;
            _temBullet = _temGo.GetComponent<BulletBase>();
            var value = _firePoint.forward * _config.BulletSpeed;
            _temBullet.Fire(new Vector3(value.z, rotateOffsetY, rotateOffsetX));

        }
        if (_temBullet && _temBullet.Config.ShellPrefab)
        {
            _temGo = Object.Instantiate(_temBullet.Config.ShellPrefab);
            _temGo.transform.SetParent(GameManager.Instance.BulletGoRoot);
            _temGo.transform.position = _clipPoint.position;
            var rig = _temGo.GetComponent<Rigidbody>();

            rig.AddForce(new Vector3(-_clipPoint.forward.z * 100, 500, (Random.value - 0.5f) * 20));
        }
        if (_config.FireEffect)
        {
            _temGo = Object.Instantiate(_config.FireEffect);
            _temGo.transform.SetParent(GameManager.Instance.EffectGoRoot);
            _temGo.transform.position = _firePoint.position;
        }
        _temBullet = null;
        return true;
    }

}
