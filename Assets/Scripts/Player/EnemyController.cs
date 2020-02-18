using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PlayerControllerBase
{
    private bool _isMoving;
    private MoveDirection _moveDir;

    public void SetMoveDir(MoveDirection dir)
    {
        _moveDir = dir;
    }

    public void StartMove()
    {
        _isMoving = true;
    }

    public void StopMove()
    {
        _isMoving = false;
    }

    public void FixedUpdate()
    {
        if (!_isMoving || _moveDir == MoveDirection.None || !_isOnGround)
            return;
        UpdateMoveAndTurn(_moveDir == MoveDirection.Left ? -1 : 1);
    }

    new protected void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (_moveDir == MoveDirection.Left)
                _moveDir = MoveDirection.Right;
            else if (_moveDir == MoveDirection.Right)
                _moveDir = MoveDirection.Left;
        }
    }
}
