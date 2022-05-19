using System.Collections.Generic;
using UnityEngine;
using System;

public enum Weapons
{
    Null,
    AK74,
    M1911,
    Uzi,
    RPG7,
    BenelliM4
}


public class Data
{
    public static WeaponData[] weapons = new WeaponData[]
    {
        new WeaponData(),

        new WeaponData            // ID 1:        AK74
        {
            weaponName = "AK74",
            weaponID = 1,
            weaponClass = WeaponClasses.AssaultRifle,
            weaponEnum = Weapons.AK74,
            weaponFireType = FireTypes.RapidFire,

            fireRateRPM = 600,
            damageRange = new Range(30, 36),
            reloadTime = 1.3f,
            magazineSize = 30,
            lowAmmoIndicator = 8,

            caliberName = "5.45×39mm",
            effectiveFiringRange = 550,
            bulletVelocity = 900,

            damageMultipliers = new BodyDamageMultipliers( new Dictionary<DamageMultipliers, float>() {
                    { DamageMultipliers.Head, 1.69f },
                    { DamageMultipliers.Chest, 1.25f }
            } ),

            falloffData = new BulletFalloffData
            {
                velocityFalloffPerSecond = 200,
                downwardsTiltAmount = 0.035f,
                damageMultiplierRemoval = 0.6f,
                lifeAfterEffectiveRangeSeconds = 0.85f
            },

            recoilData = new RecoilParameters
            {
                hipfireRecoil = new Vector3(8f, 4f, 0.35f),
                ADSRecoil = new Vector3(3f, 2f, 0.1f),
                snappiness = 2,
                returnSpeed = 6
            },

            accuracyData = new AccuracyData
            {
                hipHorzAccuracy = 3,
                hipVertAccuracy = 3,
                ADSHorzAccuracy = 0.4f,
                ADSVertAccuracy = 0.4f
            },

            hipfireADSData = new HipfireADSData
            {
                hipfirePosition = new Vector3(0.164f, -0.075f, 0.4f),
                hipfireRotation = new Vector3(0f, 180f, 0f),
                ADSPosition = new Vector3(0f, -0.045f, -0.02f),
                ADSRotation = new Vector3(0f, 180f, 0f),
                hipfireToADS = 0.2f,
                ADSToHipfire = 0.2f,
                ADSFOVIncrease = 1.35f
            }
        },


        new WeaponData            // ID 2:        1911
        {
            weaponName = "1911",
            weaponID = 2,
            weaponClass = WeaponClasses.Pistol,
            weaponEnum = Weapons.M1911,
            weaponFireType = FireTypes.SingleFire,

            fireRateRPM = 300,
            damageRange = new Range(28, 33),
            reloadTime = 1.47f,
            magazineSize = 10,
            lowAmmoIndicator = 3,

            caliberName = ".45 ACP",
            effectiveFiringRange = 50,
            bulletVelocity = 250,

            damageMultipliers = new BodyDamageMultipliers( new Dictionary<DamageMultipliers, float>() {
                    { DamageMultipliers.Head, 2f },
                    { DamageMultipliers.Chest, 1.4f }
            } ),

            falloffData = new BulletFalloffData
            {
                velocityFalloffPerSecond = 400,
                downwardsTiltAmount = 0.09f,
                damageMultiplierRemoval = 0.9f,
                lifeAfterEffectiveRangeSeconds = 0.25f
            },

            recoilData = new RecoilParameters
            {
                hipfireRecoil = new Vector3(6f, 7f, 0.35f),
                ADSRecoil = new Vector3(3f, 2f, 0.1f),
                snappiness = 6,
                returnSpeed = 2
            },

            accuracyData = new AccuracyData
            {
                hipHorzAccuracy = 5,
                hipVertAccuracy = 5,
                ADSHorzAccuracy = 0.3f,
                ADSVertAccuracy = 0.3f
            },

            hipfireADSData = new HipfireADSData
            {
                hipfirePosition = new Vector3(0.19f, -0.075f, 0.36f),
                hipfireRotation = new Vector3(0f, 180f, 0f),
                ADSPosition = new Vector3(0f, -0.045f, 0.1f),
                ADSRotation = new Vector3(0f, 180f, 0f),
                hipfireToADS = 0.1f,
                ADSToHipfire = 0.1f,
                ADSFOVIncrease = 1.35f
            }
        },


        new WeaponData            // ID 3:        Uzi
        {
            weaponName = "Uzi",
            weaponID = 3,
            weaponClass = WeaponClasses.SMG,
            weaponEnum = Weapons.Uzi,
            weaponFireType = FireTypes.RapidFire,

            fireRateRPM = 900,
            damageRange = new Range(16, 22),
            reloadTime = 1.23f,
            magazineSize = 43,
            lowAmmoIndicator = 10,

            caliberName = "9mm Parabellum",
            effectiveFiringRange = 160,
            bulletVelocity = 420,

            damageMultipliers = new BodyDamageMultipliers( new Dictionary<DamageMultipliers, float>() {
                    { DamageMultipliers.Head, 1.6f },
                    { DamageMultipliers.Chest, 1.25f },
                    { DamageMultipliers.Torso, 1.08f }
            } ),

            falloffData = new BulletFalloffData
            {
                velocityFalloffPerSecond = 420,
                downwardsTiltAmount = 0.9f,
                damageMultiplierRemoval = 0.9f,
                lifeAfterEffectiveRangeSeconds = 0.15f
            },

            recoilData = new RecoilParameters
            {
                hipfireRecoil = new Vector3(3.5f, 2f, 0.35f),
                ADSRecoil = new Vector3(2f, 2f, 0.1f),
                snappiness = 10,
                returnSpeed = 2
            },

            accuracyData = new AccuracyData
            {
                hipHorzAccuracy = 4,
                hipVertAccuracy = 4,
                ADSHorzAccuracy = 1.7f,
                ADSVertAccuracy = 1.7f
            },

            hipfireADSData = new HipfireADSData
            {
                hipfirePosition = new Vector3(0.19f, -0.095f, 0.4f),
                hipfireRotation = new Vector3(0f, 180f, 0f),
                ADSPosition = new Vector3(0f, -0.065f, 0.18f),
                ADSRotation = new Vector3(0f, 180f, 0f),
                hipfireToADS = 0.16f,
                ADSToHipfire = 0.16f,
                ADSFOVIncrease = 1.35f
            }
        },


        new WeaponData            // ID 4:        RPG7
        {
            weaponName = "RPG7",
            weaponID = 4,
            weaponClass = WeaponClasses.Launcher,
            weaponEnum = Weapons.RPG7,
            weaponFireType = FireTypes.SingleFire,

            fireRateRPM = 1000,
            damageRange = new Range(0, 0),
            reloadTime = 3.95f,
            magazineSize = 1,
            lowAmmoIndicator = 0,

            caliberName = "40mm Rockets",
            effectiveFiringRange = 10000,
            bulletVelocity = 10000,

            damageMultipliers = new BodyDamageMultipliers(),

            falloffData = new BulletFalloffData(),

            recoilData = new RecoilParameters
            {
                hipfireRecoil = new Vector3(16f, 3f, 0.35f),
                ADSRecoil = new Vector3(16f, 3f, 0.1f),
                snappiness = 10,
                returnSpeed = 2
            },

            accuracyData = new AccuracyData
            {
                hipHorzAccuracy = 0,
                hipVertAccuracy = 0,
                ADSHorzAccuracy = 0,
                ADSVertAccuracy = 0
            },

            hipfireADSData = new HipfireADSData
            {
                hipfirePosition = new Vector3(0.16f, -0.1f, 0.35f),
                hipfireRotation = new Vector3(0f, 179.6f, 0f),
                ADSPosition = new Vector3(0.0377f, -0.085f, 0.16f),
                ADSRotation = new Vector3(0f, 180f, 0f),
                hipfireToADS = 0.61f,
                ADSToHipfire = 0.61f,
                ADSFOVIncrease = 1.5f
            }
        },


        new WeaponData            // ID 5:        Benelli M4
        {
            weaponName = "Benelli M4",
            weaponID = 5,
            weaponClass = WeaponClasses.Shotgun,
            weaponEnum = Weapons.BenelliM4,
            weaponFireType = FireTypes.SingleFire,

            fireRateRPM = 300,
            damageRange = new Range(160, 200),
            reloadTime = 0.6f,
            magazineSize = 8,
            lowAmmoIndicator = 2,

            caliberName = "12 Gauge",
            effectiveFiringRange = 25f,
            bulletVelocity = 408,

            damageMultipliers = new BodyDamageMultipliers( new Dictionary<DamageMultipliers, float>() {
                    { DamageMultipliers.Head, 1.6f },
                    { DamageMultipliers.Chest, 1.25f }
            } ),

            falloffData = new BulletFalloffData
            {
                velocityFalloffPerSecond = 1000,
                downwardsTiltAmount = 0f,
                damageMultiplierRemoval = 16f,
                lifeAfterEffectiveRangeSeconds = 0.1f
            },

            recoilData = new RecoilParameters
            {
                hipfireRecoil = new Vector3(-18f, 15f, 0.8f),
                ADSRecoil = new Vector3(-18f, 5f, 0.3f),
                snappiness = 10,
                returnSpeed = 1.8f
            },

            accuracyData = new AccuracyData
            {
                hipHorzAccuracy = 3,
                hipVertAccuracy = 3,
                ADSHorzAccuracy = 0.4f,
                ADSVertAccuracy = 0.4f
            },

            hipfireADSData = new HipfireADSData
            {
                hipfirePosition = new Vector3(0.139f,-0.072f,0.408f),
                hipfireRotation = new Vector3(0f, 180f, 0f),
                ADSPosition = new Vector3(0f, -0.03f, 0.25f),
                ADSRotation = new Vector3(0f, 180f, 0f),
                hipfireToADS = 0.29f,
                ADSToHipfire = 0.29f,
                ADSFOVIncrease = 1.35f
            }
        },



    };

    public static Sound[] sounds = new Sound[]
    {

    };









    public static WeaponData GetWeaponData(Weapons weapon)
    {
        if (weapon == Weapons.Null)
            return weapons[(int) Weapons.AK74];
        return weapons[(int) weapon];
    }

    public static AmmoTypes GetAmmoType(Weapons weapon)
    {
        WeaponClasses weaponClass = weapons[(int)weapon].weaponClass;

        if (weaponClass == WeaponClasses.AssaultRifle || weaponClass == WeaponClasses.LMG)
        {
            return AmmoTypes.AssaultRifle;
        }
        else if (weaponClass == WeaponClasses.Pistol || weaponClass == WeaponClasses.SMG)
        {
            return AmmoTypes.SMG;
        }
        else if (weaponClass == WeaponClasses.MarksmanRifle || weaponClass == WeaponClasses.SniperRifle)
        {
            return AmmoTypes.Sniper;
        }
        else if (weaponClass == WeaponClasses.Shotgun)
        {
            return AmmoTypes.Shotgun;
        }
        else if (weaponClass == WeaponClasses.Launcher)
        {
            return AmmoTypes.Launcher;
        }

        return AmmoTypes.None;
    }
}

[Serializable]
public class WeaponData
{
    public string weaponName;
    public int weaponID;
    public WeaponClasses weaponClass;
    public Weapons weaponEnum;
    public FireTypes weaponFireType;

    public Range damageRange;
    public int fireRateRPM;       // rounds per minute
    
    public float reloadTime;
    public int magazineSize;
    public int lowAmmoIndicator;

    public string caliberName;
    public float effectiveFiringRange;
    public int bulletVelocity;

    public BodyDamageMultipliers damageMultipliers;
    public BulletFalloffData falloffData;
    public HipfireADSData hipfireADSData;
    public RecoilParameters recoilData;
    public AccuracyData accuracyData;
}

[Serializable]
public class BulletFalloffData
{
    public float velocityFalloffPerSecond;
    public float downwardsTiltAmount;
    public float damageMultiplierRemoval;
    public float lifeAfterEffectiveRangeSeconds;
}

[Serializable]
public class BodyDamageMultipliers
{
    public IDictionary<DamageMultipliers, float> multipliers = new Dictionary<DamageMultipliers, float>();

    public BodyDamageMultipliers(IDictionary<DamageMultipliers, float> overrides = null)
    {
        foreach (DamageMultipliers dmgMult in Enum.GetValues(typeof(DamageMultipliers)))
        {
            if (overrides != null)
            {
                if (overrides.ContainsKey(dmgMult))
                {
                    multipliers[dmgMult] = overrides[dmgMult];
                    continue;
                }
            }
            multipliers[dmgMult] = 1f;
        }
    }

    public float GetMultiplier(DamageMultipliers bodyPart)
    {
        return multipliers[bodyPart];
    }
}

[Serializable]
public class AccuracyData
{
    public float hipHorzAccuracy;
    public float hipVertAccuracy;
    public float ADSHorzAccuracy;
    public float ADSVertAccuracy;
}

[Serializable]
public class RecoilParameters
{
    public Vector3 hipfireRecoil;
    public Vector3 ADSRecoil;

    public float snappiness;
    public float returnSpeed;

}


[Serializable]
public class HipfireADSData
{
    public Vector3 hipfirePosition;
    public Vector3 hipfireRotation;

    public Vector3 ADSPosition;
    public Vector3 ADSRotation;

    public float hipfireToADS;
    public float ADSToHipfire;

    public float ADSFOVIncrease;
}

[Serializable]
public struct Range
{
    public float min { get; set; }
    public float max { get; set; }

    public Range(float minimum, float maximum)
    {
        min = minimum;
        max = maximum;
    }

    public static Range operator*(Range left, float right)
    {
        return new Range(left.min * right, left.max * right);
    }
}


public enum WeaponClasses
{
    AssaultRifle,
    SMG,
    Pistol,
    Shotgun,
    LMG,
    MarksmanRifle,
    SniperRifle,
    Launcher,
    Melee
}

public enum AmmoTypes
{
    AssaultRifle,
    SMG,
    Sniper,
    Shotgun,
    Launcher,
    None
}

public enum FireTypes
{
    SingleFire,
    RapidFire,
    BurstFire
}


public enum DamageMultipliers
{
    Head,
    Chest,
    UpperArm,
    LowerArm,
    Hand,
    Torso,
    UpperLeg,
    LowerLeg,
    Feet,
    AlwaysOne
}

[Serializable]
public class Sound
{
    public Sounds sound;
    public AudioClip audioClip;
    public Vector2 pitchRange = new Vector2(1, 1);

    public AudioClip GetAudio()
    {
        return audioClip;
    }
}

public enum Sounds
{
    AK74_Fire,
    AK74_Reload
}