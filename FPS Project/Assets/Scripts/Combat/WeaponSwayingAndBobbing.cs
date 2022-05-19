using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwayingAndBobbing : MonoBehaviour
{
    public InputMaster controls;
    public CharacterController characterController;
    public MouseLook mouseLook;

    public bool swaying;
    public bool bobbing;

    public bool ADSing;

    [Header("Swaying")]
    public float swayMultiplier;
    public float swaySmoothness;
    public float mouseMoveClamp;

    [Header("Bobbing")]
    public float bobMagnitude;
    public float bobSmoothness;
    public float velocityMagnitudeClamp;
    public float bobBaseTimeMultiplier;
    public float bobSprintTimeMultiplier;

    float bobbingTarget = 0f;
    double bobbingTime = 0f;
    Vector3 playerLastPosition;
    Vector3 playerVelocity;
    Vector3 bobbingLocation;

    private void Awake()
    {
        controls = new InputMaster();
    }

    private void Start()
    {
        playerLastPosition = characterController.transform.position;
    }


    private void Update()
    {
        if (PauseMenu.gameIsPaused || Time.deltaTime == 0f)
            return;

        playerVelocity = (characterController.transform.position - playerLastPosition) / Time.deltaTime;

        playerLastPosition = characterController.transform.position;

        if (swaying)
            SwayUpdate(ADSing ? 0.2f : 1f);
        if (bobbing)
            BobUpdate(ADSing);
    }


    private void SwayUpdate(float multiplier)
    {
        Vector2 delta = controls.Combat.Aim.ReadValue<Vector2>() / mouseLook.sensitivity * multiplier;

        float mouseX = Mathf.Clamp(delta.x, -mouseMoveClamp, mouseMoveClamp) * swayMultiplier;
        float mouseY = Mathf.Clamp(delta.y, -mouseMoveClamp, mouseMoveClamp) * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, swaySmoothness * Time.deltaTime);
    }

    private void BobUpdate(bool ADS)
    {
        float bobTimeMultiplier = bobBaseTimeMultiplier;
        if (controls.Player.Sprint.ReadValue<float>() != 0f)
        {
            bobTimeMultiplier = bobBaseTimeMultiplier * bobSprintTimeMultiplier;
        }

        bobbingTime += Time.deltaTime * bobTimeMultiplier;
        bobbingTarget = Mathf.Lerp(bobbingTarget, Mathf.Clamp(playerVelocity.magnitude, -velocityMagnitudeClamp, velocityMagnitudeClamp), bobSmoothness * Time.deltaTime);
        bobbingLocation = new Vector3(Mathf.Sin((float)bobbingTime - 1),
                                      -Mathf.Abs(Mathf.Cos((float)bobbingTime - 1)), 0f);

        Vector3 targetPosition = (ADSing ? 0.016f : 1f) * bobbingLocation * bobbingTarget * bobMagnitude;
        Vector3 lerpedPosition = Vector3.Lerp(transform.localPosition, targetPosition, bobSmoothness * Time.deltaTime * (ADSing ? 3f : 1f));

        transform.localPosition = lerpedPosition;
    }

    



    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
