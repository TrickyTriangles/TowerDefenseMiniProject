using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mob : Entity, IDamageable
{
    [SerializeField] private MobProfile mob_profile;
    [SerializeField] private EnvironmentText enviro_text;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform model;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider hitbox;

    private Transform target;
    private bool is_active;
    private int health;

    private void Start()
    {
        if (mob_profile != null)
        {
            health = mob_profile.base_health;
        }

        is_active = true;
    }

    void IDamageable.Damage(DamageProfile damage_profile)
    {
        int damage_result = DamageCalculator.CalculateDamage(damage_profile, mob_profile);

        health -= damage_result;
        enviro_text.CreateNewEnvironmentText(damage_result.ToString());

        if (health <= 0 && is_active)
        {
            KillMob();
        }
    }

    private void KillMob()
    {
        is_active = false;

        InvokeOnDeath();
        StopAllCoroutines();
        StartCoroutine(DeathAnimationRoutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hero") && target == null)
        {
            target = other.gameObject.transform;
            StartCoroutine(AttackHeroRoutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform == target)
        {
            target = null;
        }
    }

    private IEnumerator AttackHeroRoutine()
    {
        animator.Play("Attack(1h)");

        Hero hero = target.gameObject.GetComponent<Hero>();
        float timer = 0f;

        while (target != null)
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            timer += Time.deltaTime;
            model.LookAt(target);

            if (timer >= info.length)
            {
                timer -= info.length;

                if (hero != null)
                {
                    IDamageable damageable = hero;
                    damageable.Damage(mob_profile.GetDamageProfile());
                }
            }

            yield return null;
        }

        animator.Play("Idle");
        model.rotation = Quaternion.identity;
    }

    private IEnumerator DeathAnimationRoutine()
    {
        hitbox.enabled = false;
        animator.Play("Defeat");

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        float timer = 0f;

        while (timer < info.length)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}

[CreateAssetMenu(menuName = "New Mob Profile")]
public class MobProfile : ScriptableObject
{
    public DamageProfile.DamageType damage_type;
    public int base_health;
    public AnimationCurve base_power;
    public float velocity;
    [Range(0, 1)] public float normal_damage_resistance;
    [Range(0, 1)] public float slow_damage_resistance;
    [Range(0, 1)] public float slow_chance_resist;
    [Range(0, 1)] public float freeze_damage_resistance;
    [Range(0, 1)] public float freeze_chance_resist;
    [Range(0, 1)] public float burn_damage_resistance;
    [Range(0, 1)] public float burn_chance_resist;

    public DamageProfile GetDamageProfile()
    {
        DamageProfile damage_profile = new DamageProfile();

        damage_profile.damage_type = damage_type;
        damage_profile.base_damage = (int)base_power.Evaluate(1);

        return damage_profile;
    }
}