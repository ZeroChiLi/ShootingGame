using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerChangeWeaponSkill : IPlayerSkill
{
    private HeroController _heroCtrl;
    private float _interval = 1f;
    private float _nextChangeWeaponTime = 0f;

    public bool Init(object context)
    {
        _heroCtrl = context as HeroController;
        if (_heroCtrl == null)
            return false;

        return true;
    }

    public void Update(float curTime)
    {
        return;
    }

    public bool ReadyToPlay()
    {
        return _nextChangeWeaponTime < Time.time && Input.GetKey(KeyCode.Q);
    }

    public bool Play()
    {
        if (_heroCtrl.changeWeaponEffect)
        {
            GameObject _temGo = UnityEngine.Object.Instantiate(_heroCtrl.changeWeaponEffect);
            _temGo.transform.SetParent(GameManager.Instance.EffectGoRoot);
            _temGo.transform.position = _heroCtrl.GetWeaponSlot().position;
        }
        _heroCtrl.ChangeNextWeapon();
        _nextChangeWeaponTime = Time.time + _interval;
        return true;
    }

}
