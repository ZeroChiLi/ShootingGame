using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FristBullet : BulletBase
{
    private Vector3 _lastPosition;

    public override void Fire(Vector3 vec3)
    {
        base.Fire(vec3);
        _lastPosition = transform.position;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        transform.rotation = Quaternion.Euler(0, srcRotateY - transform.forward.z * _rigidbody.velocity.z, _rigidbody.velocity.y);
        RaycastHit hit;
        var curPosition = transform.position;
        var shootDir = curPosition - _lastPosition;
        Debug.DrawRay(_lastPosition, shootDir, Color.Lerp(Color.green, Color.red, Time.time - _fireTime), shootDir.magnitude * 1.1f);
        if (Physics.Raycast(_lastPosition, shootDir, out hit, shootDir.magnitude * 1.1f, ~(1 << LayerMask.NameToLayer("Bullet"))))
        {
            OnHit(hit.collider, hit.point, hit.normal);
        }
        _lastPosition = curPosition;

    }

    protected override void OnHit(Collider collider, Vector3 point, Vector3 normal)
    {
        base.OnHit(collider, point, normal);
        if (Config.HitEffectList != null && collider.sharedMaterial != null)
        {
            string materialName = collider.sharedMaterial.name;
            int count = Config.HitEffectList.Length;
            for (int i = 0; i < count; i++)
            {
                if (materialName == Config.HitEffectList[i].name && Config.HitEffectList[i].prefabs != null)
                {
                    int effectCount = Config.HitEffectList[i].prefabs.Length;
                    for (int j = 0; j < effectCount; j++)
                    {
                        SpawnDecal(collider, point, normal, Config.HitEffectList[i].prefabs[j]);
                    }
                    break;
                }
            }
        }

    }

    void SpawnDecal(Collider collider, Vector3 point, Vector3 normal, GameObject prefab)
    {
        GameObject spawnedDecal = GameObject.Instantiate(prefab, point, Quaternion.LookRotation(normal));
        spawnedDecal.transform.SetParent(collider.transform);
    }
}
