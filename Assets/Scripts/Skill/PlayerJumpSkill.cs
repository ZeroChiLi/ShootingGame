using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerJumpSkill : IPlayerSkill
{
    private HeroController _heroCtrl;

    public bool Init(object context)
    {
        _heroCtrl = context as HeroController;
        if (_heroCtrl == null)
            return false;

        return true;
    }

    public bool ReadyToPlay()
    {
        return _heroCtrl.IsOnGround && Input.GetKeyDown(KeyCode.Space);
    }

    public bool Play()
    {
        _heroCtrl.IsOnGround = false;
        _heroCtrl.Player.Rigidbody.AddForce(Vector3.up * _heroCtrl.Config.JumpSpeed);
        return true;
    }

    public void Update(float curTime)
    {
    }
}
