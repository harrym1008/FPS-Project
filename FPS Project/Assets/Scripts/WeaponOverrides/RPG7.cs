using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG7 : MonoBehaviour
{
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] MeshRenderer rocketMeshRenderer;


    public void Fire((Vector3, Vector3) worldSpace)
    {
        Instantiate(rocketPrefab, worldSpace.Item1, Quaternion.LookRotation(worldSpace.Item2));

        rocketMeshRenderer.enabled = false;
        Invoke("ShowRocket", 1.15f);
    }

    void ShowRocket()
    {
        rocketMeshRenderer.enabled = true;
    }
}
