using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WepaonFireSkill : IPlayerSkill
{
    private HeroController _heroCtrl;
    private IWeapon _curWeapon;

    public bool Init(object context)
    {
        _heroCtrl = context as HeroController;
        if (_heroCtrl == null)
            return false;
        _curWeapon = _heroCtrl.GetCurWeapon();

        return true;
    }

    public void Update(float curTime)
    {
        _heroCtrl.Player.IsLockTurn = ReadyToPlay();
    }

    public bool ReadyToPlay()
    {
        return _curWeapon != null && Input.GetKey(KeyCode.X);
    }

    public bool Play()
    {
        if (_curWeapon.Fire())
        {
            _heroCtrl.Player.Rigidbody.AddForce(-_heroCtrl.Player.transform.forward * _curWeapon.Config.Recoil);
            if (_heroCtrl.IsOnGround && _heroCtrl.fireDustEffect)
            {
                GameObject _temGo = UnityEngine.Object.Instantiate(_heroCtrl.fireDustEffect);
                _temGo.transform.SetParent(GameManager.Instance.EffectGoRoot);
                _temGo.transform.position = _heroCtrl.footPos.position;
            }
            _heroCtrl.handAnimator.SetTrigger("Fire");
            return true;
        }
        return false;
    }
}
