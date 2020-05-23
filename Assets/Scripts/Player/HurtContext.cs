
using UnityEngine;

public class HurtContext
{
    public float hurtValue;
    public float impactRange;
    public Vector3 hitPoint;
    public Vector3 hitNormal;

    public HurtContext(float hurtValue, float impactRange, Vector3 hitPoint, Vector3 hitNormal)
    {
        this.hurtValue = hurtValue;
        this.impactRange = impactRange;
        this.hitPoint = hitPoint;
        this.hitNormal = hitNormal;

    }
}
