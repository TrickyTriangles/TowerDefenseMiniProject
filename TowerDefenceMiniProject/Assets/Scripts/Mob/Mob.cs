using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mob : MonoBehaviour, IDamageable
{
    [SerializeField] private MobProfile mob_profile;
    [SerializeField] private EnvironmentText enviro_text;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform model;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider hitbox;

    public event EventHandler OnDeath;
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
        health -= damage_profile.base_damage;
        enviro_text.CreateNewEnvironmentText(damage_profile.base_damage.ToString());

        if (health <= 0 && is_active)
        {
            OnDeath?.Invoke(this, EventArgs.Empty);
            //RemoveAsTarget(damage_profile);
            is_active = false;

            StopAllCoroutines();
            StartCoroutine(DeathAnimationRoutine());
        }
    }

    private void RemoveAsTarget(DamageProfile damage_profile)
    {
        if (damage_profile.fired_by != null)
        {
            if (damage_profile.fired_by.CompareTag("Tower"))
            {
                Tower tower = damage_profile.fired_by.GetComponent<Tower>();
                tower.LoseTarget();
                return;
            }
            else if (damage_profile.fired_by.CompareTag("Hero"))
            {
                Hero hero = damage_profile.fired_by.GetComponent<Hero>();
                hero.LoseTarget();
                return;
            }
        }
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

    public DamageProfile GetDamageProfile()
    {
        DamageProfile damage_profile = new DamageProfile();

        damage_profile.damage_type = damage_type;
        damage_profile.base_damage = (int)base_power.Evaluate(1);

        return damage_profile;
    }
}