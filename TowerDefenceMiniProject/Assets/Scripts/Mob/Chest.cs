using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Entity, IDamageable
{
    [SerializeField] private EnvironmentText enviro_text;
    [SerializeField] private GameObject coin;

    void IDamageable.Damage(DamageProfile damage_profile)
    {
        enviro_text.CreateNewEnvironmentText("Hit!");

        if (coin != null)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
        }
    }
}
