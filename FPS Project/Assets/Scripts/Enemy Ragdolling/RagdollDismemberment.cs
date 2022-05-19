using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollDismemberment : MonoBehaviour
{
    [System.Serializable]
    public class DismembermentData
    {
        public DismemberableLimbs dismemberedLimb;
        public Transform limbTransform;

        public float bloodAmount;
        public float bloodTime;

        public Vector3 localPSPosition;
        public Vector3 localPSRotation;
    }

    [SerializeField] DismembermentData[] dismemberments;
    public List<DismemberableLimbs> limbsToDismember = new List<DismemberableLimbs>();
    public GameObject bloodObject;


    public void Dismember()
    {
        foreach (DismemberableLimbs limb in limbsToDismember)
        {
            if (RNG.RandomBoolean())
            {
                continue;
            }

            foreach (DismembermentData data in dismemberments)
            {
                if (limb == data.dismemberedLimb)
                {
                    data.limbTransform.localScale = new Vector3(0f, 0f, 0f);

                    ParticleSystem blood = Instantiate(bloodObject).GetComponent<ParticleSystem>();
                    blood.transform.parent = data.limbTransform.parent;

                    blood.transform.localPosition = data.localPSPosition;
                    blood.transform.localEulerAngles = data.localPSRotation;

                    ParticleSystem.EmissionModule emission = blood.emission;
                    emission.rateOverTime = data.bloodAmount * RNG.Range(0.9f, 1.1f);

                    ParticleSystem.MainModule main = blood.main;
                    main.duration = data.bloodTime * RNG.Range(0.9f, 1.1f);

                    blood.Play();
                }
            }
        }
    }

}

public enum DismemberableLimbs
{
    None,
    Head,
    LeftUpperArm,
    LeftLowerArm,
    RightUpperArm,
    RightLowerArm,
    LeftThigh,
    LeftCalf,
    RightThigh,
    RightCalf
}