using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieToRagdoll : MonoBehaviour
{
    public bool iAmRagdoll;
    public RagdollData myTransforms;
    public RagdollData ragdollData;
    public GameObject ragdollObject;


    public void Start()     // Ragdoll spawned
    {
        myTransforms.CreateArray();

        if (!iAmRagdoll)
            return;

        for (int i = 0; i < ragdollData.allTransforms.Length; i++)
        {
            myTransforms.allTransforms[i].position = ragdollData.allTransforms[i].position;
            myTransforms.allTransforms[i].rotation = ragdollData.allTransforms[i].rotation;
        }
    }


    public void IHaveDied(List<DismemberableLimbs> limbsToDismember)     // Enemy dies
    {
        if (!iAmRagdoll)
        {
            DieToRagdoll ragdoll = Instantiate(ragdollObject).GetComponent<DieToRagdoll>();
            myTransforms.CreateArray();
            ragdoll.ragdollData = myTransforms;
            Destroy(gameObject);

            RagdollDismemberment dismemberment = ragdoll.GetComponent<RagdollDismemberment>();
            dismemberment.limbsToDismember = limbsToDismember;
            dismemberment.Dismember();
        }
    }
}




[System.Serializable]
public class RagdollData
{
    public Transform parentLocation;
    public Transform limbPelvis;
    public Transform limbLThigh;
    public Transform limbLCalf;
    public Transform limbLFoot;
    public Transform limbRThigh;
    public Transform limbRCalf;
    public Transform limbRFoot;
    public Transform limbSpine1;
    public Transform limbSpine2;
    public Transform limbRibcage;
    public Transform limbLCollarbone;
    public Transform limbLUpperArm;
    public Transform limbLForeArm;
    public Transform limbLHand;
    public Transform limbRCollarbone;
    public Transform limbRUpperArm;
    public Transform limbRForeArm;
    public Transform limbRHand;
    public Transform limbNeck;
    public Transform limbHead;

    public Transform[] allTransforms;


    public void CreateArray()
    {
        allTransforms = new Transform[21]
        {
             parentLocation,
             limbPelvis,
             limbLThigh,
             limbLCalf,
             limbLFoot,
             limbRThigh,
             limbRCalf,
             limbRFoot,
             limbSpine1,
             limbSpine2,
             limbRibcage,
             limbLCollarbone,
             limbLUpperArm,
             limbLForeArm,
             limbLHand,
             limbRCollarbone,
             limbRUpperArm,
             limbRForeArm,
             limbRHand,
             limbNeck,
             limbHead
        };
    }
}
