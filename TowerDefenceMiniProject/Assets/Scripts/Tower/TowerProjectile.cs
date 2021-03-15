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
    private Entity entity;

    private void Entity_OnDeath(object sender, EventArgs e)
    {
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

        transform.position = Vector3.MoveTowards(transform.position, target.position + offset, velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                if (entity != null)
                    entity.OnDeath -= Entity_OnDeath;

                damageable.Damage(damage_profile);
            }

            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform new_target)
    {
        target = new_target;
        entity = target.gameObject.GetComponent<Entity>();

        if (entity != null)
        {
            entity.OnDeath += Entity_OnDeath;
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
    public float damage_modifier;
}