using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBase : MonoBehaviour, IPlayer
{
    [SerializeField] protected PlayerConfig _config;
    //[SerializeField] protected GameObject _normalModel;
    [SerializeField] protected GameObject _deadModel;
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected ColorEffect _hurtEffect;

    protected PlayerControllerBase _controller;
    public PlayerConfig Config { get => _config; }
    public float HP { get; protected set; }
    public PlayerState State { get; protected set; }
    public bool IsLockTurn { get; set; }
    public Rigidbody Rigidbody { get => _rigidbody; }
    public Animator Animator { get => _animator; }
    private LinkedList<HurtContext> _hurtList = new LinkedList<HurtContext>();

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

    virtual protected void GetHurt(HurtContext context)
    {
        _hurtList.AddFirst(context);
        HP -= context.hurtValue;
        if (_hurtEffect)
        {
            _hurtEffect.Play();
        }
        if (State != PlayerState.Dead && HP <= 0)
        {
            State = PlayerState.Dead;
            OnDead(context);
        }
        else
        {
            var power = (Vector3.up * 0.3f + context.hitPoint - transform.position).normalized * context.impactRange;
            //Debug.Log($"受击！！{name}  :  {power} {(context.hitPoint - transform.position).normalized} {context.impactRange}");
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.AddForce(power);

        }
    }

    virtual protected void OnDead(HurtContext context)
    {
        _controller.SetIsDead(true);
        Rigidbody.isKinematic = true;
        //_normalModel.gameObject.SetActive(false);
        var deadModel = GameObject.Instantiate(_deadModel, GameManager.Instance.DeadModelGoRoot);
        deadModel.name = $"DeadModel_{gameObject.name}";
        deadModel.transform.position = transform.position;
        deadModel.transform.rotation = transform.rotation;
        deadModel.transform.localScale = transform.localScale;
        var deadCom = deadModel.GetComponent<PlayerDeadModel>();
        if (deadCom)
        {
            deadCom.Play(_hurtList);
        }

    }
}
