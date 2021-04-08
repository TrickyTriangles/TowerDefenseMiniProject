using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hero : MonoBehaviour, IDamageable
{
    [SerializeField] private HeroProfile hero_profile;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform model;
    [SerializeField] private int health;
    [SerializeField] private int level;
    private Transform target;
    private GameManager.HeroKilledMobEvent killed_mob_event;
    private Action<Hero, int> hero_gained_experience_event;

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    private void Start()
    {
        StartCoroutine(StartupRoutine());
    }

    private void Entity_OnDeath(object sender, EventArgs e)
    {
        target = null;

        var onDeathEventArgs = e as Entity.OnDeathEventArgs;

        if (onDeathEventArgs.last_hit.fired_by != null)
        {
            Hero hero = onDeathEventArgs.last_hit.fired_by.GetComponent<Hero>();

            if (hero != null)
            {               
                if (sender is Mob mob)
                {
                    killed_mob_event?.Invoke(mob._experience, hero);
                }
            }
        }
    }

    public void LoseTarget()
    {
        target = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target == null && other.gameObject.CompareTag("Targetable"))
        {
            target = other.gameObject.transform;
            StartCoroutine(AttackTargetRoutine());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (target == null && other.gameObject.CompareTag("Targetable"))
        {
            target = other.gameObject.transform;
            StartCoroutine(AttackTargetRoutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform == target)
        {
            target = null;
        }
    }

    private DamageProfile GetDamageProfile()
    {
        DamageProfile new_profile = new DamageProfile();

        new_profile.fired_by = gameObject;
        new_profile.damage_type = hero_profile.damage_type;
        new_profile.base_damage = (int)hero_profile.shot_power.Evaluate(level);

        return new_profile;
    }

    private void SetAttackingAnimation()
    {
        if (hero_profile != null && animator != null)
        {
            switch (hero_profile.attack_animation)
            {
                case HeroProfile.AttackAnimationType.ATTACK_1H:
                    animator.Play("Attack(1h)");
                    break;
                case HeroProfile.AttackAnimationType.HEAVY_ATTACK:
                    animator.Play("HeavyAttack");
                    break;
                case HeroProfile.AttackAnimationType.SHOOT:
                    animator.Play("Shoot(1h)");
                    break;
                default:
                    break;
            }
        }
    }

    void IDamageable.Damage(DamageProfile damage_profile)
    {
        if (EnvironmentText.IsInitialized)
        {
            EnvironmentText.Instance.DrawText(damage_profile.base_damage.ToString(), EnvironmentText.TextTypes.NORMAL, transform.position);
        }
    }

    private IEnumerator StartupRoutine()
    {
        // Wait for GameManager class to be initialized before using it.
        while (GameManager.Instance.HeroKilledMob == null)
        {
            yield return null;
        }

        killed_mob_event += GameManager.Instance.HeroKilledMob;
        hero_gained_experience_event += GameManager.Instance.HeroGainedExperienceEvent;
    }

    private IEnumerator AttackTargetRoutine()
    {
        Entity entity = target.GetComponent<Entity>();
        IDamageable damageable = target.GetComponent<IDamageable>();
        float timer = 0f;

        SetAttackingAnimation();
        if (entity != null) { entity.OnDeath += Entity_OnDeath; }

        while (target != null)
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            timer += Time.deltaTime;
            model.LookAt(target);

            if (timer >= info.length)
            {
                if (damageable != null)
                {
                    damageable.Damage(GetDamageProfile());
                    hero_gained_experience_event?.Invoke(this, 1);
                }

                timer -= info.length;
            }

            yield return null;
        }

        if (entity != null) { entity.OnDeath -= Entity_OnDeath; }
        animator.Play("Idle");
    }
}
