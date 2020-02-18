using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : PlayerControllerBase
{
    [SerializeField] private Transform _weaponSlot;
    [SerializeField] private WeaponBase _curWeapon;
    [SerializeField] private GameObject _fireDustEffect;
    [SerializeField] private Animator _handAnimator;


    public void FixedUpdate()
    {
        if (_player.State == PlayerState.Dead)
            return;
        UpdateFire();
        var inputX = Input.GetAxis("Horizontal");
        UpdateMoveAndTurn(inputX);
        UpdateJump();
        CameraController.Instance.SetTarget(_player.transform, GetCurDirection());
    }

    private void UpdateJump()
    {
        if (_isOnGround && Input.GetKeyDown(KeyCode.Space))
        {
            _isOnGround = false;
            _player.Rigidbody.AddForce(Vector3.up * Config.JumpSpeed);
        }
    }

    //private GameObject _temGo;
    private void UpdateFire()
    {
        if (_curWeapon && Input.GetKey(KeyCode.LeftShift))
        {
            if (_curWeapon.Fire())
            {
                _player.Rigidbody.AddForce(-_player.transform.forward * _curWeapon.Config.Recoil);
                if (_isOnGround && _fireDustEffect)
                {
                    GameObject _temGo = Object.Instantiate(_fireDustEffect);
                    _temGo.transform.SetParent(GameManager.Instance.EffectGoRoot);
                    _temGo.transform.position = _footPos.position;
                }
                _handAnimator.SetTrigger("Fire");
            }
            _player.IsFighting = true;
        }
        else
        {
            _player.IsFighting = false;
        }
    }
}
