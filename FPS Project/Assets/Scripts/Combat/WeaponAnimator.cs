using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    public enum Parameters
    {
        Reload,
        ADS,
        Firing
    }

    public GameObject[] weaponModels;
    public Weapons currentWeapon;
    public Animator[] weaponAnimators;

    Transform[] muzzleFlashes;
    ParticleSystem[] bulletCasings;
    ParticleSystem[] reloadSmoke;

    List<Transform> fireLocations = new List<Transform>() { null };
    List<Transform> backOfBarrels = new List<Transform>() { null };
    [SerializeField] LayerMask barrelCheckLayerMask;

    AudioSource sfxSource;
    [SerializeField] Sound[] sounds;



    private void Start()
    {
        sfxSource = GetComponent<AudioSource>();

        currentWeapon = Weapons.AK74;

        List<ParticleSystem> releaseCasings = new List<ParticleSystem>();
        List<ParticleSystem> _reloadSmoke = new List<ParticleSystem>();
        List<Transform> flashParents = new List<Transform>();

        int currentAnimator = 0;


        foreach (Animator animator in weaponAnimators)
        {
            if (currentAnimator != 0)
            {
                fireLocations.Add(animator.transform.Find("FireLocation"));
                backOfBarrels.Add(animator.transform.Find("BackOfBarrel"));
            }

            try
            {
                flashParents.Add(animator.transform.Find("MuzzleFlash"));
            }
            catch (System.Exception)
            {
                flashParents.Add(null);
            }

            try
            {
                releaseCasings.Add(animator.transform.Find("ReleaseCasing").GetComponent<ParticleSystem>());
            }
            catch (System.Exception)
            {
                releaseCasings.Add(null);
            }

            try
            {
                _reloadSmoke.Add(animator.transform.Find("ReloadSmoke").GetComponent<ParticleSystem>());
            }
            catch (System.Exception)
            {
                _reloadSmoke.Add(null);
            }

            currentAnimator++;
        }

        muzzleFlashes = flashParents.ToArray();
        bulletCasings = releaseCasings.ToArray();
        reloadSmoke = _reloadSmoke.ToArray();


        foreach (ParticleSystem PS in reloadSmoke)
        {
            if (PS == null)
            {
                continue;
            }

            ParticleSystem.MainModule main = PS.main;
            main.simulationSpace = ParticleSystemSimulationSpace.Custom;
            main.customSimulationSpace = transform;
        }
    }


    public void ChangeWeaponModel(int showModel)
    {
        currentWeapon = (Weapons)showModel;
        showModel--;

        for (int i = 0; i < weaponModels.Length; i++)
        {
            if (showModel == i)
            {
                weaponModels[i].SetActive(true);
            }
            else
            {
                weaponModels[i].SetActive(false);
            }
        }
    }



    public void UpdateVariable(Parameters param, bool value)
    {
        weaponAnimators[(int)currentWeapon].SetBool(param.ToString(), value);

        if (param == Parameters.Firing && value == true && currentWeapon != Weapons.Null)
        {
            sfxSource.pitch = RNG.RangeBetweenVector2(sounds[0].pitchRange);
            sfxSource.PlayOneShot(sounds[0].GetAudio());

            try {  bulletCasings[(int)currentWeapon].Play();  } catch { }

            for (int i = 0; i < muzzleFlashes[(int)currentWeapon].childCount; i++)
            {
                try
                {
                    muzzleFlashes[(int)currentWeapon].GetChild(i).GetComponent<ParticleSystem>().Play();
                }
                catch { }                
            }
        }
        else if (param == Parameters.Reload && value == true && currentWeapon != Weapons.Null)
        {
            try { reloadSmoke[(int)currentWeapon].Play(); } catch { }

            sfxSource.pitch = RNG.RangeBetweenVector2(sounds[1].pitchRange);
            sfxSource.PlayOneShot(sounds[1].GetAudio());
        }
    }




    public (Vector3, Vector3) ProjectileWorldSpawnData(Weapons weapon)
    {
        int intWeapon = (int)weapon;
        Vector3 fireLocation = fireLocations[intWeapon].position;

        bool hitWall = Physics.Linecast(backOfBarrels[intWeapon].position, fireLocation, barrelCheckLayerMask);

        if (hitWall)
        {
            fireLocation = backOfBarrels[intWeapon].position;
        }

        return (fireLocation, -fireLocations[intWeapon].forward);
    }
}


