using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDebrisManager : MonoBehaviour
{
    [System.Serializable]
    public class DebrisType
    {
        [Header("First 4 letters of the material")] public string debrisCode;
        public GameObject debrisParticles;
    }

    [SerializeField] private DebrisType[] debris;
    public string myDebrisCode = "none";

    private void Start()
    {
        myDebrisCode = myDebrisCode.Substring(0, 4);

        foreach (DebrisType instance in debris)
        {
            if (instance.debrisCode != myDebrisCode)
            {
                Destroy(instance.debrisParticles);
            }
        }
    }
}
