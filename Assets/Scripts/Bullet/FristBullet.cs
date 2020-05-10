using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FristBullet : BulletBase
{
    protected override void OnHit(RaycastHit hit)
    {
        base.OnHit(hit);
        if (Config.HitEffectList != null && hit.collider.sharedMaterial != null)
        {
            string materialName = hit.collider.sharedMaterial.name;
            int count = Config.HitEffectList.Length;
            for (int i = 0; i < count; i++)
            {
                if (materialName == Config.HitEffectList[i].name && Config.HitEffectList[i].prefabs!= null)
                {
                    int effectCount = Config.HitEffectList[i].prefabs.Length;
                    for (int j = 0; j < effectCount; j++)
                    {
                        SpawnDecal(hit, Config.HitEffectList[i].prefabs[j]);
                    }
                    break;
                }
            }
        }
        
    }

    void SpawnDecal(RaycastHit hit, GameObject prefab)
    {
        GameObject spawnedDecal = GameObject.Instantiate(prefab, hit.point, Quaternion.LookRotation(hit.normal));
        spawnedDecal.transform.SetParent(hit.collider.transform);
    }
}
