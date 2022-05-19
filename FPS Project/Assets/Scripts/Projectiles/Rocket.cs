using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] LayerMask collidesWith;
    [SerializeField] float speed = 100f;
    [SerializeField] float lifetime = 5f;

    [SerializeField] GameObject explosionPrefab;

    bool active = false;


    void LateUpdate()
    {
        lifetime -= Time.deltaTime;
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        active = true;

        if (lifetime <= 0)
        {
            Explode();
        }
    }


    void Explode()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        print("Boom!");

        Destroy(gameObject);
    }




    private void OnTriggerEnter(Collider other)
    {
        print(other);

        if (active && CheckLayerInMask(other.gameObject.layer))
        {
            Explode();
        }
    }


    private bool CheckLayerInMask(int layer)
    {
        return (collidesWith.value & (1 << layer)) > 1;
    }
}
