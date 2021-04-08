using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class Mob : Entity, IDamageable
{
    [SerializeField] protected MobProfile mob_profile;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform model;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected CapsuleCollider hitbox;
    [SerializeField] protected EffectProcessor effect_processor;
    protected Transform target;
    public bool is_active;

    private int health;
    public int _health
    {
        get { return health; }
        set { health = value; }
    }

    public int _base_health
    {
        get { return mob_profile.base_health; }
    }

    public int _experience
    {
        get { return mob_profile.experience >= 0 ? mob_profile.experience : 1; }
    }

    private float velocity;
    public float _velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }

    public float _base_velocity
    {
        get { return mob_profile != null ? mob_profile.velocity : 1f; }
    }

    public EffectProcessor _effect_processor
    {
        get { return effect_processor; }
    }

    public Animator _animator
    {
        get { return animator; }
    }

    public float _animation_playback_speed
    {
        set { AdjustAnimationSpeed(value); }
        get { return animator.speed; }
    }

    private void Start()
    {
        if (mob_profile != null)
        {
            health = mob_profile.base_health;
            _velocity = mob_profile.velocity;
        }

        is_active = true;
        animator.Play("Run");
    }

    private void AdjustAnimationSpeed(float speed)
    {
        if (animator != null)
        {
            animator.speed = speed;
        }
    }

    void IDamageable.Damage(DamageProfile damage_profile)
    {
        HitMob(damage_profile);
    }

    protected void HitMob(DamageProfile damage_profile)
    {
        DamageCalculator.DamageCalculationResult result;
        DamageCalculator.CalculateDamage(damage_profile, mob_profile, this, out result);

        for (int i = 0; i < result.damage_effects.Count; i++)
        {
            effect_processor.StartNewEffect(result.damage_effects[i]);
        }

        if (EnvironmentText.IsInitialized)
        {
            EnvironmentText.TextTypes type;

            switch (result.damage_type)
            {
                case DamageProfile.DamageType.NORMAL:
                    type = EnvironmentText.TextTypes.NORMAL;
                    break;
                case DamageProfile.DamageType.SLOW:
                    type = EnvironmentText.TextTypes.SLOW;
                    break;
                case DamageProfile.DamageType.FREEZE:
                    type = EnvironmentText.TextTypes.FREEZE;
                    break;
                case DamageProfile.DamageType.BURN:
                    type = EnvironmentText.TextTypes.BURN;
                    break;
                default:
                    type = EnvironmentText.TextTypes.NORMAL;
                    break;
            }

            EnvironmentText.Instance.DrawText(result.damage_total.ToString(), type, transform.position);
        }

        ReduceHealth(result.damage_total, damage_profile);
    }

    public void ReduceHealth(int value, DamageProfile damage_profile)
    {
        health -= value;

        if (health <= 0 && is_active)
        {
            KillMob(damage_profile);
        }
    }

    protected virtual void KillMob(DamageProfile last_hit)
    {
        OnDeathEventArgs args = new OnDeathEventArgs();
        args.last_hit = last_hit;
        InvokeOnDeath(args);

        is_active = false;
        _animation_playback_speed = 1f;
        effect_processor.StopAllCoroutines();
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
            if (_animation_playback_speed > 0f) { timer += Time.deltaTime; }
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

        animator.Play("Run");
        model.rotation = Quaternion.identity;
    }

    private IEnumerator DeathAnimationRoutine()
    {
        hitbox.enabled = false;
        animator.Play("Defeat", 0, 0f);

        yield return new WaitForEndOfFrame();

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        while (info.normalizedTime < 1f)
        {
            info = animator.GetCurrentAnimatorStateInfo(0);

            yield return null;
        }

        Destroy(gameObject);
    }
}