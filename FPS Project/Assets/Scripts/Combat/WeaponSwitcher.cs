using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] ADSManager ADSManager;
    [SerializeField] Recoil Recoil;
    [SerializeField] PlayerWeapons PlayerWeapons;
    [SerializeField] WeaponAnimator WeaponAnimator;


    private void Start()
    {
        InstantSwitch(1);
    }



    public void LateUpdate()
    {
        int queuedWeaponChange = -1;

        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown((KeyCode) 256 + i) || Input.GetKeyDown((KeyCode) 48 + i))
            {
                queuedWeaponChange = i;
            }
        }


        if (queuedWeaponChange != -1)
        {
            InstantSwitch(queuedWeaponChange);
        }        
    }


    void InstantSwitch(int switchTo)
    {
        Weapons weapon = (Weapons) switchTo;
        
        ADSManager.UpdateADSData(weapon);
        Recoil.UpdateRecoilData(weapon);
        PlayerWeapons.UpdateWeaponData(weapon);
        WeaponAnimator.ChangeWeaponModel(switchTo);
    }
}