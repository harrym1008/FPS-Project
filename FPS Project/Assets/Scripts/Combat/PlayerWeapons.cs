using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public GunDataUI gunUI;
    private Recoil recoilScript;
    public WeaponSwayingAndBobbing weaponSwayingAndBobbing;

    public GameObject basicBullet;
    public GameObject basicShell;

    public InputMaster controls;
    public WeaponAnimator weaponAnimator;

    public GameObject overrideScripts;


    public float reloadTimeRemaining;
    public float timeBetweenShots;


    public int bulletsInMag;
    public int[] backupAmmo;
    public static int[] maxBackupAmmo = new int[] { 300, 250, 50, 50, 20, int.MaxValue };

    // 0: Assault Rifle / LMG Ammo
    // 1: Pistol / SMG Ammo
    // 2: Sniper / Marksman Rifle Ammo
    // 3: Shotgun Ammo
    // 4: Launcher Ammo
    // 5: None

    public Weapons currentWeapon;
    public WeaponData currentWeaponData;

    bool queueReload;
    bool singleFire;

    

    private void Start()
    {
        UpdateWeaponData(currentWeapon);
        //backupAmmo = new int[] { 0, 0, 0, 0, 0, int.MaxValue };

        recoilScript = transform.Find("CameraRot/CameraRecoil").GetComponent<Recoil>();
        recoilScript.UpdateRecoilData(Weapons.AK74);

        gunUI.UpdateBackupRounds(backupAmmo[(int)Data.GetAmmoType(currentWeapon)], currentWeapon);
        gunUI.UpdateRoundsInMagazine(bulletsInMag, currentWeapon);
        gunUI.UpdateLowAmmoText(currentWeapon, bulletsInMag, backupAmmo[(int)Data.GetAmmoType(currentWeapon)]);
    }


    public void UpdateWeaponData(Weapons weapon)
    {
        currentWeapon = weapon;
        currentWeaponData = Data.GetWeaponData(weapon);
        gunUI.UpdateGunNameAndCaliber(currentWeaponData.weaponName, currentWeaponData.caliberName);
    }
    

    public IEnumerator Reload()
    {
        if (backupAmmo[(int)Data.GetAmmoType(currentWeapon)] == 0 || bulletsInMag == currentWeaponData.magazineSize)
        {
            yield break;
        }

        reloadTimeRemaining = currentWeaponData.reloadTime;
        weaponAnimator.UpdateVariable(WeaponAnimator.Parameters.Reload, true);
        yield return new WaitForSeconds(reloadTimeRemaining * 0.1f);
        weaponAnimator.UpdateVariable(WeaponAnimator.Parameters.Reload, false);

        yield return new WaitForSeconds(reloadTimeRemaining * 0.75f);

        int ammoRequired = currentWeaponData.magazineSize - bulletsInMag;

        if (ammoRequired > backupAmmo[(int) Data.GetAmmoType(currentWeapon)])
        {
            bulletsInMag += backupAmmo[(int)Data.GetAmmoType(currentWeapon)];
            backupAmmo[(int)Data.GetAmmoType(currentWeapon)] = 0;
        }
        else
        {
            bulletsInMag = currentWeaponData.magazineSize;
            backupAmmo[(int)Data.GetAmmoType(currentWeapon)] -= ammoRequired;
        }

        gunUI.UpdateBackupRounds(backupAmmo[(int)Data.GetAmmoType(currentWeapon)], currentWeapon);
        gunUI.UpdateRoundsInMagazine(bulletsInMag, currentWeapon);
    }




    private void Update()
    {
        reloadTimeRemaining -= Time.deltaTime;
        timeBetweenShots -= Time.deltaTime;

        if (controls.Combat.Reload.ReadValue<float>() != 0)
        {
            queueReload = true;
        } 

        if (queueReload && timeBetweenShots <= 0 && reloadTimeRemaining <= 0)
        {
            StartCoroutine(Reload());
            queueReload = false;
            singleFire = false;
            return;
        }        

        bool firing = false;

        if (currentWeaponData.weaponFireType == FireTypes.SingleFire)
        {
            if (singleFire && CheckIHaveBullets())
                if (Fire())
                    firing = true;
        }
        else
        {
            if (controls.Combat.Fire.ReadValue<float>() != 0 && CheckIHaveBullets())
                if (Fire())
                    firing = true;
        }       


        if (!firing)
            weaponAnimator.UpdateVariable(WeaponAnimator.Parameters.Firing, false);



        singleFire = false;
    }


    private void SingleFire()
    {
        singleFire = true;
    }


    private bool Fire()
    {
        if (reloadTimeRemaining >= 0 || timeBetweenShots >= 0 || PauseMenu.gameIsPaused)
        {
            return false;
        }
        else if (bulletsInMag <= 0)
        {
            StartCoroutine(Reload());
            return false;
        }

        weaponAnimator.UpdateVariable(WeaponAnimator.Parameters.Firing, true);
        bulletsInMag--;

        gunUI.UpdateRoundsInMagazine(bulletsInMag, currentWeapon);
        gunUI.UpdateLowAmmoText(currentWeapon, bulletsInMag, backupAmmo[(int)Data.GetAmmoType(currentWeapon)]);
    
        CreateProjectile();

        timeBetweenShots = 60f / currentWeaponData.fireRateRPM;

        recoilScript.RecoilFire();

        if (bulletsInMag <= 0)
        {
            queueReload = true;
        }

        return true;
    }

    void CreateProjectile()
    {
        (Vector3, Vector3) worldSpace = weaponAnimator.ProjectileWorldSpawnData(currentWeapon);

        if (currentWeapon == Weapons.RPG7)
        {
            overrideScripts.GetComponent<RPG7>().Fire(worldSpace);
            return;
        }
        else if (currentWeapon == Weapons.BenelliM4)
        {
            ShellBehaviour shell = Instantiate(basicShell, null).GetComponent<ShellBehaviour>();
            shell.weapon = (int) currentWeapon;

            shell.transform.position = worldSpace.Item1;
            shell.transform.rotation = Quaternion.LookRotation(worldSpace.Item2);

            return;
        }

        BulletBehaviour bullet = Instantiate(basicBullet, null).GetComponent<BulletBehaviour>();
        bullet.weaponFiredData = currentWeapon;
        bullet.spawnedWhileADS = recoilScript.ADSing;

        bullet.transform.position = worldSpace.Item1;
        bullet.transform.rotation = Quaternion.LookRotation(worldSpace.Item2);
    }




    private bool CheckIHaveBullets()
    {
        return !(bulletsInMag + backupAmmo[(int)Data.GetAmmoType(currentWeapon)] <= 0);
    }




    private void Awake()
    {
        controls = new InputMaster();
        controls.Combat.Fire.started += _ => SingleFire();
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