using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public const float CRIT = 1.5f;


    public int health;
    public int armour;

    HealthArmourUI healthArmourUI;

    public bool dead = false;
    public bool isPlayer = false;

    public DismemberableLimbs limbToDismember = DismemberableLimbs.None;


    private void Start()
    {
        if (isPlayer)
            healthArmourUI = FindObjectOfType<HealthArmourUI>();
    }



    public void TakeDamage(float damage, DismemberableLimbs? dismemberLimb = null)
    {
        if (dead)
        {
            return;
        }


        (int, int) dmg = GetDamage(damage);

        health -= dmg.Item1;
        armour -= dmg.Item2;

        if (health <= 0)
        {
            dead = true;

            if (!isPlayer)
            {
                //print(dismemberLimb);

                if (dismemberLimb == null)
                {
                    limbToDismember = DismemberableLimbs.None;
                }
                else
                {
                    limbToDismember = (DismemberableLimbs) dismemberLimb;
                }

            }
        }

        if (isPlayer)
        {
            healthArmourUI.health = health;
            healthArmourUI.armour = armour;
        }

        try
        {
            GetComponent<AudioSource>().Play();
        }
        catch { }
    }






    (int, int) GetDamage(float damage)
    {
        int intDMG = Mathf.RoundToInt(damage);

        int healthDMG = 0, armourDMG = 0;

        if (intDMG > armour)
        {
            armourDMG = armour;
            healthDMG = intDMG - armourDMG;
        }
        else
        {
            armourDMG = intDMG;
        }


        if (healthDMG > health)
        {
            healthDMG = health;
        }

        return (healthDMG, armourDMG);
    }
}
