using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunDataUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundsInMag;
    [SerializeField] private TextMeshProUGUI backupAmmo;
    [SerializeField] private TextMeshProUGUI gunName;
    [SerializeField] private TextMeshProUGUI caliberName;
    [SerializeField] private TextMeshProUGUI lowNoAmmoText;


    public void UpdateRoundsInMagazine(int rounds, Weapons weapon)
    {
        roundsInMag.text = $"<mspace=73m>{rounds}";
        if (rounds <= Data.GetWeaponData(weapon).lowAmmoIndicator)
        {
            roundsInMag.color = new Color32(255, 81, 0, 255);
        }
        else
        {
            roundsInMag.color = Color.white;
        }
    }

    public void UpdateBackupRounds(int rounds, Weapons weapon)
    {
        backupAmmo.text = $"{rounds}";
        if (rounds == 0)
        {
            backupAmmo.color = new Color32(255, 81, 0, 255);
        }
        else if (rounds <= Data.GetWeaponData(weapon).lowAmmoIndicator)
        {
            backupAmmo.color = new Color32(255, 205, 0, 255);
        }
        else
        {
            backupAmmo.color = Color.white;
        }
    }

    public void UpdateGunNameAndCaliber(string gun, string caliber)
    {
        gunName.text = gun;
        caliberName.text = caliber;
    }

    public void UpdateLowAmmoText(Weapons weapon, int rounds, int backupAmmo)
    {
        if (rounds + backupAmmo == 0)
        {
            lowNoAmmoText.text = "NO AMMO";
            lowNoAmmoText.color = new Color32(255, 81, 0, 255);
        }
        else if (rounds + backupAmmo <= Data.GetWeaponData(weapon).lowAmmoIndicator)
        {
            float yellowNess = (rounds + backupAmmo) / (float)Data.GetWeaponData(weapon).lowAmmoIndicator;

            lowNoAmmoText.text = "LOW AMMO";
            lowNoAmmoText.color = new Color32(255, (byte) Mathf.RoundToInt(101 + 104 * yellowNess), 0, 255);
        }
        else
        {
            lowNoAmmoText.text = "";
        }
    }
}
