using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BulletBehaviour : MonoBehaviour
{
    float effectiveRange;
    float velocity;
    Range damageRange;

    float velocityFalloff;
    float falloffTiltAmount;
    float damageFalloff;

    [Header("Aesthetics data")]
    public GameObject smokeEffect;
    public GameObject bloodEffect;
    public GameObject bulletTrail;
    public Light bulletLight;

    bool killing;

    [Header("Bullet falloff data")]
    [SerializeField] float lifetimeRemaining;
    [SerializeField] float remainingRange;
    [SerializeField] float currentVelocity;
    [SerializeField] float damageMultiplier = 1f;

    [Header("Other data")]
    [SerializeField] LayerMask layerMask;
    BulletTrailFade trailLine;

    bool frame1 = true;

    public bool spawnedWhileADS;
    public Weapons weaponFiredData;
    public GameObject bulletHole;
    public GameObject dmgPopup;

    [SerializeField] WeaponData weaponData;

    void Start()
    {
        weaponData = Data.GetWeaponData(weaponFiredData);

        effectiveRange = weaponData.effectiveFiringRange;
        velocity = weaponData.bulletVelocity;
        damageRange = weaponData.damageRange;

        velocityFalloff = weaponData.falloffData.velocityFalloffPerSecond;
        falloffTiltAmount = weaponData.falloffData.downwardsTiltAmount;
        damageFalloff = weaponData.falloffData.damageMultiplierRemoval;
        lifetimeRemaining = weaponData.falloffData.lifeAfterEffectiveRangeSeconds;


        if (spawnedWhileADS)
            transform.eulerAngles += new Vector3(RNG.RangePosNeg(weaponData.accuracyData.ADSVertAccuracy),
                                                 RNG.RangePosNeg(weaponData.accuracyData.ADSHorzAccuracy), 0f);
        else
            transform.eulerAngles += new Vector3(RNG.RangePosNeg(weaponData.accuracyData.hipVertAccuracy),
                                                 RNG.RangePosNeg(weaponData.accuracyData.hipHorzAccuracy), 0f);


        remainingRange = effectiveRange;
        currentVelocity = velocity;

        trailLine = Instantiate(bulletTrail, transform.position, Quaternion.identity, null).GetComponent<BulletTrailFade>();
        trailLine.Assign();
        trailLine.lr.SetPosition(0, transform.position);
        trailLine.lr.SetPosition(1, transform.position);
    }
    
    void LateUpdate()
    {
        if (frame1 || killing)
        {
            frame1 = false;
            return;
        }


        float distanceThisFrame = currentVelocity * Time.deltaTime;
        remainingRange -= distanceThisFrame;

        Obstruction obstruction = CheckForObstruction(distanceThisFrame);

        if (trailLine != null)
        {
            trailLine.lr.SetPosition(1, obstruction.advanceTo);
        }

        if (obstruction.hitSomething)
        {
            if (obstruction.collider.TryGetComponent(out TargetReferral targetReferral))
            {
                DamageMultipliers dmgMult = targetReferral.GetDamageMultiplier();
                float baseDMG = RNG.Range(damageRange * damageMultiplier);
                baseDMG *= weaponData.damageMultipliers.multipliers[dmgMult];
                float dmgTaken = targetReferral.TakeDamage(baseDMG);

                TextMeshPro popUp = Instantiate(dmgPopup, obstruction.advanceTo, Quaternion.identity).transform.GetChild(0).GetComponent<TextMeshPro>();
                popUp.text = Mathf.RoundToInt(dmgTaken).ToString();
                popUp.transform.parent.DOMove(popUp.transform.parent.position + RNG.RandomVector3() * 0.8f, 0.5f);
                    
                if (weaponData.damageMultipliers.multipliers[dmgMult] * targetReferral.staticDamageMultiplier < Target.CRIT)
                    popUp.GetComponent<Animator>().SetBool("Crit", false);
                else
                    popUp.GetComponent<Animator>().SetBool("Crit", true);

                Destroy(popUp.transform.parent.gameObject, 1.1f);

                GameObject blood = Instantiate(bloodEffect, obstruction.raycastHit.point, Quaternion.LookRotation(obstruction.raycastHit.normal), obstruction.raycastHit.collider.transform);
                blood.GetComponent<ParticleSystem>().Play();
                Destroy(blood, 0.6f);

           //     print($"Bullet of name '{gameObject.name}' hit {obstruction.collider.name}: dmgMult is {dmgMult.ToString()}");
            }

            transform.localScale = Vector3.zero;
            Destroy(gameObject, 1.5f);
            Destroy(bulletLight.gameObject);
            killing = true;

            GameObject smoke = Instantiate(smokeEffect, obstruction.raycastHit.point, Quaternion.LookRotation(obstruction.raycastHit.normal));
            Destroy(smoke, 1.5f);

            if (trailLine != null)
            {
                trailLine.speed *= 2;
            }

            try
            {
                RaycastHit hit = obstruction.raycastHit;
                MeshCollider meshCollider = hit.collider as MeshCollider;
                if (meshCollider != null || meshCollider.sharedMesh != null)
                {
                    Mesh mesh = meshCollider.sharedMesh;
                    Renderer renderer = hit.collider.GetComponent<MeshRenderer>();

                    int[] hitTriangle = new int[]
                    {
                            mesh.triangles[hit.triangleIndex * 3],
                            mesh.triangles[hit.triangleIndex * 3 + 1],
                            mesh.triangles[hit.triangleIndex * 3 + 2]
                    };
                    for (int i = 0; i < mesh.subMeshCount; i++)
                    {
                        int[] subMeshTris = mesh.GetTriangles(i);
                        for (int j = 0; j < subMeshTris.Length; j += 3)
                        {
                            if (subMeshTris[j] == hitTriangle[0] &&
                                subMeshTris[j + 1] == hitTriangle[1] &&
                                subMeshTris[j + 2] == hitTriangle[2])
                            {
                                Material mat = renderer.materials[i];
                                smoke.GetComponent<BulletDebrisManager>().myDebrisCode = mat.name.Substring(0, 4);
                                //print(smoke.GetComponent<BulletDebrisManager>().myDebrisCode);
                            }
                        }
                    }

                }

                Destroy(Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal) * Quaternion.Euler(RNG.RandomVector3(0f, 0f, 180f))), 120);

            }
            catch { }

            return;
        }
        else
        {
            transform.position = obstruction.advanceTo;
        }


        if (remainingRange < 0)
        {
            currentVelocity -= velocityFalloff * Time.deltaTime;
            damageMultiplier -= Time.deltaTime * damageFalloff;
            lifetimeRemaining -= Time.deltaTime;

            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles,
                new Vector3(90, transform.eulerAngles.y, 0f)
                , falloffTiltAmount * Time.deltaTime);

            if (lifetimeRemaining <= 0f/* || currentVelocity < 0 || damageMultiplier < 0*/)
            {
                Destroy(gameObject);
            }
        }
    }



    private Obstruction CheckForObstruction(float distanceToAdvance)
    {
        Vector3 endingPosition = transform.position + transform.forward * distanceToAdvance;
        bool didIHit = Physics.Linecast(transform.position, endingPosition, out RaycastHit hit, layerMask);

        if (didIHit)
        {
            return new Obstruction
            {
                hitSomething = true,
                raycastHit = hit,
                collider = hit.collider,
                advanceTo = hit.point
            };
        }
        else
        {
            return new Obstruction
            {
                hitSomething = false,
                raycastHit = hit,
                collider = null,
                advanceTo = endingPosition
            };
        }
    }
}

public class Obstruction
{
    public bool hitSomething;
    public RaycastHit raycastHit;
    public Collider collider = null;
    public Vector3 advanceTo = new Vector3();
}