using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : PlayerControllerBase
{
    public GoGhostEffect ghostEffect;
    public GameObject fireDustEffect;
    public GameObject changeWeaponEffect;
    public Animator handAnimator;
    [SerializeField] private Transform _weaponSlot;
    [SerializeField] private WeaponBase _curWeapon;
    [SerializeField] private WeaponBase[] _weaponArr;
    private int _curWeaponIndex = 0;

    protected List<IPlayerSkill> playerSkills = new List<IPlayerSkill>()
    {
        new PlayerJumpSkill(),
        new PlayerSprintSkill(),
        new WepaonFireSkill(),
        new PlayerChangeWeaponSkill(),
    };
    private int _skillCount;

    override protected void Awake()
    {
        base.Awake();
        _skillCount = playerSkills.Count;
        if (_curWeapon == null && _weaponArr != null && _weaponArr.Length > 0)
        {
            _curWeapon = _weaponArr[0];
        }
    }

    protected void Start()
    {
        for (int i = 0; i < _skillCount; i++)
        {
            playerSkills[i].Init(this);
        }
    }

    public void FixedUpdate()
    {
        if (Player.State == PlayerState.Dead)
            return;
        for (int i = 0; i < _skillCount; i++)
        {
            var curTime = Time.time;
            playerSkills[i].Update(curTime);
        }
        for (int i = 0; i < _skillCount; i++)
        {
            if (playerSkills[i].ReadyToPlay())
            {
                playerSkills[i].Play();
            }
        }
        var inputX = Input.GetAxis("Horizontal");
        UpdateMoveAndTurn(inputX);
        CameraController.Instance.SetTarget(Player.transform, GetCurDirection());
    }
    
    public IWeapon GetCurWeapon()
    {
        return _curWeapon;
    }

    public IWeapon ChangeNextWeapon()
    {
        _curWeapon?.gameObject.SetActive(false);
        ++_curWeaponIndex;
        _curWeaponIndex %= _weaponArr.Length;
        _curWeapon = _weaponArr[_curWeaponIndex];
        _curWeapon?.gameObject.SetActive(true);
        return _curWeapon;
    }

    public Transform GetWeaponSlot()
    {
        return _weaponSlot;
    }
}
