using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerBase))]
public class PlayerControllerBase : MonoBehaviour
{
    public Transform footPos;
    [SerializeField] private GameObject _walkDustEffect;
    [SerializeField] private AudioSource _idleSound;
    [SerializeField] private AudioSource _walkSound;

    public PlayerBase Player { get; protected set; }
    public PlayerConfig Config { get => Player.Config; }

    public bool IsOnGround { get; set; }
    public bool IsDead { get; protected set; }

    readonly private Vector3 TurnLeft = new Vector3(0, -90f, 0);
    readonly private Vector3 TurnRight = new Vector3(0, 90f, 0);
    private bool _needTurn = false;
    private Vector3 _turnDir;

    virtual protected void Awake()
    {
        Player = GetComponent<PlayerBase>();
    }

    //private GameObject _temGo;
    private Vector3 _temPos;
    private Vector3 _temEuler;
    private float _temAbsEulerY;
    protected void UpdateMoveAndTurn(float moveX)
    {
        if (IsDead)
            return;
        if (Mathf.Abs(moveX) >= 0.001f)
        {
            _temPos = transform.position;
            Player.Rigidbody.velocity = new Vector3(moveX * Config.MoveSpeed, Player.Rigidbody.velocity.y, Player.Rigidbody.velocity.z);

            _temEuler = transform.localRotation.eulerAngles;

            _temAbsEulerY = Mathf.Abs(_temEuler.y);
            if ((moveX > 0 && (_temEuler.y < 0 || _temEuler.y > 180)) ||
                (moveX < 0 && (_temEuler.y > 0 && _temEuler.y < 180)) ||
                (_temAbsEulerY <= 89f || _temAbsEulerY >= 91f))
            {
                _needTurn = true;
                _turnDir = moveX > 0 ? TurnRight : TurnLeft;
            }
            else
            {
                _needTurn = false;
                transform.localRotation = Quaternion.Euler(_temEuler.y > 0 ? TurnRight : TurnLeft);
            }

            if (IsOnGround)
            {
                if (_walkDustEffect)
                {
                    GameObject _temGo = Object.Instantiate(_walkDustEffect);
                    _temGo.transform.SetParent(GameManager.Instance.EffectGoRoot);
                    _temGo.transform.position = footPos.position;
                }
                PlayWalkSound(true);
            }
            else
            {
                PlayWalkSound(false);
            }
            Player.Animator.SetBool("IsWalking", true);
        }
        else
        {
            PlayWalkSound(false);
            Player.Animator.SetBool("IsWalking", false);
        }
        if (!Player.IsLockTurn && _needTurn)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(_turnDir), Time.deltaTime * Config.TurnSpeed);
        }
    }

    protected void PlayWalkSound(bool isWalk)
    {
        if (isWalk)
        {
            if (_walkSound && !_walkSound.isPlaying)
                _walkSound.Play();
            if (_idleSound && _idleSound.isPlaying)
                _idleSound.Stop();
        }
        else
        {
            if (_walkSound && _walkSound.isPlaying)
                _walkSound.Stop();
            if (_idleSound && !_idleSound.isPlaying)
                _idleSound.Play();

        }

    }

    /// <summary>
    /// 获取当前任务朝向
    /// </summary>
    /// <returns></returns>
    public MoveDirection GetCurDirection()
    {
        _temEuler = transform.localRotation.eulerAngles;
        if (_temEuler.y < 0 || _temEuler.y > 180)
        {
            return MoveDirection.Left;
        }
        else if (_temEuler.y > 0 && _temEuler.y < 180)
        {
            return MoveDirection.Right;
        }
        return MoveDirection.None;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (IsDead)
            return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            IsOnGround = true;
        }
    }

    public void SetIsDead(bool isDead)
    {
        IsDead = isDead;
    }
}
