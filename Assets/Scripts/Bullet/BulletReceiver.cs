using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BulletReceiver : MonoBehaviour
{
    private Action<BulletBase, Vector3> _onHit;

    /// <summary>
    /// 绑定回调
    /// </summary>
    /// <param name="onHitCallback"></param>
    public void Bind(Action<BulletBase, Vector3> onHitCallback)
    {
        _onHit = onHitCallback;
    }

    /// <summary>
    /// 解绑
    /// </summary>
    public void Unbind()
    {
        _onHit = null;
    }

    /// <summary>
    /// 命中回调
    /// </summary>
    /// <param name="hitPoint"></param>
    public void OnHit(BulletBase bullet, Vector3 hitPoint)
    {
        _onHit?.Invoke(bullet, hitPoint);
    }

}