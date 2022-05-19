using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ADSManager : MonoBehaviour
{
    [SerializeField] private PlayerWeapons playerWeapons;
    [SerializeField] private WeaponSwayingAndBobbing bobbing;
    [SerializeField] private CrosshairUI crosshairUI;
    [SerializeField] private Recoil recoilManager;

    public HipfireADSData hipfireADSData;
    [SerializeField] float baseFOV = 80;

    float timeSinceLast;
    bool lastModeWasADS;

    Tween tween1;
    Tween tween2;
    Tween tween3;



    private void Start()
    {
        hipfireADSData = Data.GetWeaponData(Weapons.AK74).hipfireADSData;
        baseFOV = Camera.main.fieldOfView;

        transform.localPosition = hipfireADSData.hipfirePosition;
        transform.localEulerAngles = hipfireADSData.hipfireRotation;
    }

    private void Update()
    {
        timeSinceLast += Time.deltaTime;

        bool wantToADS = playerWeapons.controls.Combat.AimDownSight.ReadValue<float>() != 0;

        if (playerWeapons.reloadTimeRemaining >= 0)
        {
            wantToADS = false;
        }

        lastModeWasADS = ChangeMode(wantToADS);
    }


    public void UpdateADSData(Weapons weapon)
    {
        hipfireADSData = Data.GetWeaponData(weapon).hipfireADSData;

        transform.localPosition = hipfireADSData.hipfirePosition;
        transform.localEulerAngles = hipfireADSData.hipfireRotation;
    }



    bool ChangeMode(bool wantToADS)
    {
        if (lastModeWasADS == wantToADS)
        {
            return lastModeWasADS;
        }

        float moveTime = wantToADS ? hipfireADSData.hipfireToADS : hipfireADSData.ADSToHipfire;

        if (timeSinceLast < moveTime)
        {
            float temp = timeSinceLast;
            timeSinceLast = moveTime;
            moveTime = temp;
        }

        try
        {
            tween1.Kill();
            tween2.Kill();
            tween3.Kill();
        }
        catch { }

        if (wantToADS)
        {
            tween1 = transform.DOLocalMove(hipfireADSData.ADSPosition, moveTime);
            tween2 = transform.DOLocalRotate(hipfireADSData.ADSRotation, moveTime);
            tween3 = Camera.main.DOFieldOfView(baseFOV / hipfireADSData.ADSFOVIncrease, moveTime);
        }
        else
        {
            tween1 = transform.DOLocalMove(hipfireADSData.hipfirePosition, moveTime);
            tween2 = transform.DOLocalRotate(hipfireADSData.hipfireRotation, moveTime);
            tween3 = Camera.main.DOFieldOfView(baseFOV, moveTime);
        }

        recoilManager.ADSing = wantToADS;
        bobbing.ADSing = wantToADS;
        crosshairUI.renderCrosshair = !wantToADS;
        playerWeapons.weaponAnimator.UpdateVariable(WeaponAnimator.Parameters.ADS, wantToADS);

        return wantToADS;
    }
}



