using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunShadowMovement : MonoBehaviour
{ 
    [SerializeField] Transform[] gunTransforms;
    [SerializeField] Transform mainCamera;

    [SerializeField] Vector2 maxAngles;

    int currentWeapon = 0;

    public Vector3[] maxUpPositions;
    public Vector3[] maxDownPositions;
    public Vector3[] middlePositions;

    public Vector3[] maxUpRotations;
    public Vector3[] maxDownRotations;
    public Vector3[] middleRotations;


    private void Update()
    {
        float cameraAngle = mainCamera.eulerAngles.x;
        cameraAngle = (cameraAngle > 180) ? cameraAngle - 360 : cameraAngle;


        if (cameraAngle > 0)     // Downwards
        {
            float multiplier = Mathf.Abs(cameraAngle / maxAngles.x);
            gunTransforms[currentWeapon].localEulerAngles = Vector3.Lerp(middleRotations[currentWeapon],
                maxDownRotations[currentWeapon], multiplier);
            gunTransforms[currentWeapon].localPosition = Vector3.Lerp(middlePositions[currentWeapon],
                maxDownPositions[currentWeapon], multiplier);
        }
        else if (cameraAngle < 0)    // Upwards
        {
            float multiplier = Mathf.Abs(cameraAngle / maxAngles.y);
            gunTransforms[currentWeapon].localEulerAngles = Vector3.Lerp(middleRotations[currentWeapon],
                maxUpRotations[currentWeapon], multiplier);
            gunTransforms[currentWeapon].localPosition = Vector3.Lerp(middlePositions[currentWeapon],
                maxUpPositions[currentWeapon], multiplier);
        }
        else                          // Exactly zero, somehow
        {
            gunTransforms[currentWeapon].localEulerAngles = middleRotations[currentWeapon];
            gunTransforms[currentWeapon].localPosition = middlePositions[currentWeapon];
        }

    }
}