using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Entity, IDamageable
{
    [SerializeField] private GameObject coin;

    void IDamageable.Damage(DamageProfile damage_profile)
    {
        EnvironmentText.Instance.DrawText(damage_profile.fired_by.name + " hit me!", EnvironmentText.TextTypes.SLOW, transform.position);

        if (coin != null)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
        }
    }
}
