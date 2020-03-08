using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerSprintSkill : PlayerColdDownSkillBase
{
    private HeroController _heroCtrl;
    private GoGhostEffect _ghostEffect;

    public override bool Init(object context)
    {
        base.Init(context);
        _heroCtrl = context as HeroController;
        if (_heroCtrl == null)
            return false;

        _ghostEffect = _heroCtrl.ghostEffect;

        coldDownTime = _heroCtrl.Config.SprintColdDownTime;
        return true;
    }

    public override bool ReadyToPlay()
    {
        return base.ReadyToPlay() && Input.GetKeyDown(KeyCode.LeftShift);
    }

    public override bool Play()
    {
        base.Play();
        _heroCtrl.Player.Rigidbody.AddForce(_heroCtrl.transform.forward * _heroCtrl.Config.SprintSpeed);
        if (_ghostEffect)
        {
            _ghostEffect.Init(_heroCtrl.gameObject, null);
            _ghostEffect.Restore();
            _ghostEffect.Play();
        }
        return true;
    }
}
