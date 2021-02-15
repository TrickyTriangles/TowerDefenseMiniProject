using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    [SerializeField] private float velocity;

    private DamageProfile damage_profile;
    private Transform target;
    private Vector3 offset;
    private Mob mob;

    private void Mob_OnDeath(object sender, EventArgs e)
    {
        Debug.Log("Tower projectile has received OnDeath event!");
        Destroy(gameObject);
    }

    private void Start()
    {
        offset = new Vector3(0f, 0.25f, 0f);
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction_to_target = ((target.position + offset) - transform.position).normalized;
        transform.Translate(direction_to_target * velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                if (mob != null)
                    mob.OnDeath -= Mob_OnDeath;

                damageable.Damage(damage_profile);
                Destroy(gameObject);
            }
        }
    }

    public void SetTarget(Transform new_target)
    {
        target = new_target;
        mob = target.gameObject.GetComponent<Mob>();

        if (mob != null)
        {
            mob.OnDeath += Mob_OnDeath;
        }
    }

    public void SetDamageProfile(DamageProfile new_profile)
    {
        damage_profile = new_profile;
    }
}

[System.Serializable]
public struct DamageProfile
{
    public enum DamageType
    {
        NORMAL,
        SLOW,
        FREEZE,
        BURN
    }

    public GameObject fired_by;
    public DamageType damage_type;
    public int base_damage;
    public int damage_modifier;
}