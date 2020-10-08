using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WepaonFireSkill : IPlayerSkill
{
    private HeroController _heroCtrl;

    public bool Init(object context)
    {
        _heroCtrl = context as HeroController;
        if (_heroCtrl == null)
            return false;

        return true;
    }

    public void Update(float curTime)
    {
        _heroCtrl.Player.IsLockTurn = ReadyToPlay();
    }

    public bool ReadyToPlay()
    {
        return _heroCtrl.GetCurWeapon() != null && Input.GetKey(KeyCode.X);
    }

    public bool Play()
    {
        IWeapon curWeapon = _heroCtrl.GetCurWeapon();
        if (curWeapon != null && curWeapon.Fire())
        {
            _heroCtrl.Player.Rigidbody.AddForce(-_heroCtrl.Player.transform.forward * curWeapon.Config.Recoil);
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
