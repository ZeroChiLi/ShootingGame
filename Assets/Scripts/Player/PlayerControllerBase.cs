using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerBase))]
public class PlayerControllerBase : MonoBehaviour
{
    [SerializeField] protected Transform _footPos;
    [SerializeField] private GameObject _walkDustEffect;
    [SerializeField] private AudioSource _idleSound;
    [SerializeField] private AudioSource _walkSound;

    protected PlayerBase _player;
    public PlayerConfig Config { get => _player.Config; }

    protected bool _isOnGround = false;
    protected bool _isDead = false;

    readonly private Vector3 TurnLeft = new Vector3(0, -90f, 0);
    readonly private Vector3 TurnRight = new Vector3(0, 90f, 0);
    private bool _needTurn = false;
    private Vector3 _turnDir;

    protected void Awake()
    {
        _player = GetComponent<PlayerBase>();
    }

    //private GameObject _temGo;
    private Vector3 _temPos;
    private Vector3 _temEuler;
    private float _temAbsEulerY;
    protected void UpdateMoveAndTurn(float moveX)
    {
        if (_isDead)
            return;
        if (Mathf.Abs(moveX) >= 0.001f)
        {
            _temPos = transform.position;
            _player.Rigidbody.velocity = new Vector3(moveX * Config.MoveSpeed, _player.Rigidbody.velocity.y, _player.Rigidbody.velocity.z);

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

            if (_isOnGround)
            {
                if (_walkDustEffect)
                {
                    GameObject _temGo = Object.Instantiate(_walkDustEffect);
                    _temGo.transform.SetParent(GameManager.Instance.EffectGoRoot);
                    _temGo.transform.position = _footPos.position;
                }
                PlayWalkSound(true);
            }
            else
            {
                PlayWalkSound(false);
            }
            _player.Animator.SetBool("IsWalking", true);
        }
        else
        {
            PlayWalkSound(false);
            _player.Animator.SetBool("IsWalking", false);
        }
        if (!_player.IsFighting && _needTurn)
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
        if (_isDead)
            return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isOnGround = true;
        }
    }

    public void SetIsDead(bool isDead)
    {
        _isDead = isDead;
    }
}
