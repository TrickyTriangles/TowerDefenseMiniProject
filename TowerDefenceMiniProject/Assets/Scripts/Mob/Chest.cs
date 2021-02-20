using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Entity, IDamageable
{
    [SerializeField] private EnvironmentText enviro_text;
    [SerializeField] private GameObject coin;

    void IDamageable.Damage(DamageProfile damage_profile)
    {
        enviro_text.CreateNewEnvironmentText(damage_profile.fired_by.name + " hit me!");

        if (coin != null)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
        }
    }
}
