using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetReferral : MonoBehaviour
{
    [SerializeField] DamageMultipliers damageMultiplier = DamageMultipliers.Torso;
    [SerializeField] DismemberableLimbs thisLimb = DismemberableLimbs.None;

    public Target target;
    public TargetReferral targetReferral = null;

    public float staticDamageMultiplier = 1f;


    public DamageMultipliers GetDamageMultiplier()
    {
        return damageMultiplier;
    }

    public float TakeDamage(float damage, DismemberableLimbs? limb = null)
    {
        if (limb == null)
        {
            limb = thisLimb;
        }

        damage *= staticDamageMultiplier;

        if (targetReferral != null)
        {
            targetReferral.TakeDamage(damage, limb);
            return damage;
        }

        target.TakeDamage(damage, limb);
        return damage;
    }

    public Target GetMainTarget()
    {
        if (target != null)
        {
            return target;
        }
        else
        {
            return targetReferral.GetMainTarget();
        }
    }
}
