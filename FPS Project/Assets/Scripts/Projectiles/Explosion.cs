using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Explosion : MonoBehaviour
{
    public float radius = 5f;
    public int maxDamage = 400;
    public int baseDamage = 250;
    public float explosionForce = 10f;
    public float damageDecreaseFactor = 5f;

    public Light explosionLight;
    public LayerMask damagingLayerMask;
    public LayerMask ragdollLayerMask;

    public GameObject dmgPopup;

    private void Start()
    {
        StartCoroutine(ExplosionLight());
        Invoke("Explode", 0f);
        Destroy(gameObject, 5f);
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, damagingLayerMask);

        foreach (Collider enemy in colliders)
        {
            if (enemy.TryGetComponent(out Target target))
            {
                float maximumDMG = maxDamage * RNG.Range(0.9f, 1.1f);

                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                float damage;

                if (distance == 0f)
                    damage = maximumDMG;
                else
                    damage = baseDamage / (distance / radius * damageDecreaseFactor);

                if (damage > maximumDMG)
                    damage = maximumDMG;

                target.TakeDamage(damage);


                if (target.isPlayer)
                    continue;

                TextMeshPro popUp = Instantiate(dmgPopup, enemy.transform.position, Quaternion.identity).transform.GetChild(0).GetComponent<TextMeshPro>();
                popUp.text = Mathf.RoundToInt(damage).ToString();
                popUp.transform.parent.DOMove(popUp.transform.parent.position + RNG.RandomVector3() * 2f, 0.5f);

                if (damage / (baseDamage / 4) < Target.CRIT)
                    popUp.GetComponent<Animator>().SetBool("Crit", false);
                else
                    popUp.GetComponent<Animator>().SetBool("Crit", true);

                Destroy(popUp.transform.parent.gameObject, 1.1f);
            }
        }

        Invoke("ColliderForce", 0f);
    }


    void ColliderForce()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, ragdollLayerMask);

        foreach (Collider enemy in colliders)
        {
            if (enemy.TryGetComponent(out Rigidbody rb))
            {
                float distance = Vector3.Distance(enemy.transform.position, transform.position) / radius;
                if (distance < 0.1f) { distance = 0.1f; }
                rb.AddExplosionForce(explosionForce / distance, transform.position, radius);
            }
        }

    }





    IEnumerator ExplosionLight()
    {
        float maxIntensity = radius / 5 * 800 * RNG.Range(0.9f, 1.1f);

        DOTween.Sequence()
            .Append(explosionLight.DOIntensity(maxIntensity, 0.1f))
            .Append(explosionLight.DOIntensity(maxIntensity / 4, 0.3f))
            .Append(explosionLight.DOIntensity(0f, 0.9f));

        while (true)
        {
            Color color = new Color32(255, (byte) RNG.Next(90, 158), 0, 255);
            explosionLight.DOColor(color, 0.08f);
            explosionLight.transform.DOLocalMove(new Vector3(RNG.Range(-0.1f, 0.1f), RNG.Range(0.4f, 0.6f), RNG.Range(-0.1f, 0.1f)), 0.08f);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
