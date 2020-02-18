using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBase : MonoBehaviour, IPlayer
{
    [SerializeField] protected PlayerConfig _config;
    [SerializeField] protected GameObject _normalModel;
    [SerializeField] protected PlayerDeadModel _deadModel;
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected ColorEffect _hurtEffect;

    protected PlayerControllerBase _controller;
    public PlayerConfig Config { get => _config; }
    public float HP { get; protected set; }
    public PlayerState State { get; protected set; }
    public bool IsFighting { get; set; }
    public Rigidbody Rigidbody { get => _rigidbody; }
    public Animator Animator { get => _animator; }

    protected void Awake()
    {
        _controller = GetComponent<PlayerControllerBase>();
        _rigidbody = GetComponent<Rigidbody>();
        HP = Config.MaxHP;
    }

    protected void Start()
    {
        State = PlayerState.Normal;
    }

    protected void Update()
    {
    }                               

    virtual protected void GetHurt(float value, Vector3 src, float range)
    {
        HP -= value;
        if (_hurtEffect)
        {
            _hurtEffect.Play();
        }
        if (State != PlayerState.Dead && HP <= 0)
        {
            State = PlayerState.Dead;
            OnDead(src, range);
        }
        else
        {
            Rigidbody.AddForce((src - transform.position).normalized * range * 20);

        }
    }

    virtual protected void OnDead(Vector3 hurtSrc, float range)
    {
        _controller.SetIsDead(true);
        Rigidbody.isKinematic = true;
        _normalModel.gameObject.SetActive(false);
        _deadModel.gameObject.SetActive(true);
        _deadModel.Play(range * 100, hurtSrc, range);
    }
}
