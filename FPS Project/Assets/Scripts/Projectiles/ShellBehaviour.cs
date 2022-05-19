using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ShellBehaviour : MonoBehaviour
{
    public class ShellDamagePopUp
    {
        public Target target;
        public GameObject damagePopup;
        public int damageTaken;
        public bool isCrit;
    }


    public GameObject pelletObject;
    public GameObject dmgPopup;

    public int numberOfPellets = 9;
    public Vector2 positiveSpreadLimit;

    public List<PelletBehaviour> pellets = new List<PelletBehaviour>();
    public List<ShellDamagePopUp> shellDamagePopUps = new List<ShellDamagePopUp>();

    public int weapon;
    WeaponData weaponData;

    [Header("Pellet range and damage data")]
    public float maximumRange;
    public float maxDamageDistance;
    public Vector2 damageRange;
    public Vector2 damageRandomness;

    public Vector2 velocityRandomness;
    public Vector2 effectiveRangeRandomness;



    void Start()
    {
        weaponData = Data.GetWeaponData(Weapons.BenelliM4);

        for (int i = 0; i < numberOfPellets; i++)
        {
            Vector3 offset = new Vector3(RNG.RangePosNeg(positiveSpreadLimit.x), 
                                         RNG.RangePosNeg(positiveSpreadLimit.y), 0f);
            if (i == 0)
                offset = Vector3.zero;


            PelletBehaviour pelletBehaviour = Instantiate(pelletObject, transform.position, Quaternion.Euler(transform.eulerAngles + offset)).GetComponent<PelletBehaviour>();

            pelletBehaviour.parentShell = this;
            pelletBehaviour.SetData(weaponData.bulletVelocity * RNG.RangeBetweenVector2(velocityRandomness),
                           weaponData.effectiveFiringRange * RNG.RangeBetweenVector2(effectiveRangeRandomness));

            pellets.Add(pelletBehaviour);
        }
    }


    public void ReportDamage(PelletBehaviour.PelletImpact pelletImpact)
    {
        if (pelletImpact.impactCollider.TryGetComponent(out TargetReferral targetReferral))
        {
            float baseDMG = GetPelletDamage(pelletImpact) * RNG.RangeBetweenVector2(damageRandomness);

            DamageMultipliers dmgMult = targetReferral.GetDamageMultiplier();
            baseDMG *= weaponData.damageMultipliers.multipliers[dmgMult];
            float dmgTaken = targetReferral.TakeDamage(baseDMG);


            bool createNew = true;
            int index = 0;

            for (int i = 0; i < shellDamagePopUps.Count; i++)
            {
                if (shellDamagePopUps[i].target == targetReferral.GetMainTarget())
                {
                    index = i;
                    createNew = false;
                    break;
                }
            }


            if (createNew)
            {
                bool isCrit = false;

                TextMeshPro popUp = Instantiate(dmgPopup, pelletImpact.impactLocation, Quaternion.identity).transform.GetChild(0).GetComponent<TextMeshPro>();
                popUp.text = Mathf.RoundToInt(dmgTaken).ToString();
                popUp.transform.parent.DOMove(popUp.transform.parent.position + RNG.RandomVector3() * 0.8f, 0.5f);

                if (weaponData.damageMultipliers.multipliers[dmgMult] * targetReferral.staticDamageMultiplier < Target.CRIT)
                    popUp.GetComponent<Animator>().SetBool("Crit", false);
                else
                {
                    popUp.GetComponent<Animator>().SetBool("Crit", true);
                    isCrit = true;
                }

                Destroy(popUp.transform.parent.gameObject, 1.1f);

                shellDamagePopUps.Add(new ShellDamagePopUp { isCrit = isCrit,
                                                             damagePopup = popUp.transform.parent.gameObject,
                                                             damageTaken = Mathf.RoundToInt(dmgTaken),
                                                             target = targetReferral.GetMainTarget()
                });
            }
            else
            {
                Destroy(shellDamagePopUps[index].damagePopup);

                shellDamagePopUps[index].damageTaken += Mathf.RoundToInt(dmgTaken);

                TextMeshPro popUp = Instantiate(dmgPopup, pelletImpact.impactLocation, Quaternion.identity).transform.GetChild(0).GetComponent<TextMeshPro>();
                popUp.text = Mathf.RoundToInt(shellDamagePopUps[index].damageTaken).ToString();
                popUp.transform.parent.DOMove(popUp.transform.parent.position + RNG.RandomVector3() * 0.8f, 0.5f);

                if (weaponData.damageMultipliers.multipliers[dmgMult] * targetReferral.staticDamageMultiplier < Target.CRIT || !shellDamagePopUps[index].isCrit)
                    popUp.GetComponent<Animator>().SetBool("Crit", false);
                else
                {
                    popUp.GetComponent<Animator>().SetBool("Crit", true);
                    shellDamagePopUps[index].isCrit = true;
                }

                Destroy(popUp.transform.parent.gameObject, 1.1f);

                shellDamagePopUps[index].damagePopup = popUp.transform.parent.gameObject;
            }
        }
    }


    public float GetPelletDamage(PelletBehaviour.PelletImpact pelletImpact)
    {
        float distance = Vector3.Distance(pelletImpact.spawnLocation, pelletImpact.impactLocation);

        if (distance < maxDamageDistance)
        {
            return damageRange.x;
        }
        else
        {
            float multiplier = (distance - maxDamageDistance) / (maximumRange - maxDamageDistance);

            if (multiplier < 0 || multiplier > 1)
                return 1f;

            return Mathf.Lerp(damageRange.x, damageRange.y, multiplier);
        }
    }

    private void Update()
    {
        bool kill = true;

        foreach (var item in pellets)
        {
            if (item != null)
            {
                kill = false;
                break;
            }
        }

        if (kill)
        {
            Destroy(gameObject);
        }
    }
}





