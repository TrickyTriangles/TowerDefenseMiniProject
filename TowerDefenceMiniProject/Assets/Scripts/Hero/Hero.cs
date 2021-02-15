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
    [SerializeField] private EnvironmentText enviro_text;
    [SerializeField] private int level;
    private Transform target;

    private void Mob_OnDeath(object sender, EventArgs e)
    {
        Debug.Log("Hero class has received OnDeath event!");
        target = null;
    }

    public void LoseTarget()
    {
        target = null;
    }

    private DamageProfile GetDamageProfile()
    {
        DamageProfile new_profile = new DamageProfile();

        new_profile.fired_by = gameObject;
        new_profile.damage_type = hero_profile.damage_type;
        new_profile.base_damage = (int)hero_profile.shot_power.Evaluate(level);

        return new_profile;
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
        if (enviro_text != null)
        {
            enviro_text.CreateNewEnvironmentText(damage_profile.base_damage.ToString());
        }
    }

    private IEnumerator AttackTargetRoutine()
    {
        Mob mob = target.GetComponent<Mob>();
        IDamageable damageable = target.GetComponent<IDamageable>();
        float timer = 0f;

        SetAttackingAnimation();
        if (mob != null) { mob.OnDeath += Mob_OnDeath; }

        while (target != null)
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            timer += Time.deltaTime;
            model.LookAt(target);

            if (timer >= info.length)
            {
                if (damageable != null) { damageable.Damage(GetDamageProfile()); }
                timer -= info.length;
            }

            yield return null;
        }

        if (mob != null) { mob.OnDeath -= Mob_OnDeath; }
        animator.Play("Idle");
    }
}

[CreateAssetMenu(menuName = "New Hero Profile")]
public class HeroProfile : ScriptableObject
{
    public enum AttackAnimationType
    {
        ATTACK_1H,
        HEAVY_ATTACK,
        SHOOT
    }

    public DamageProfile.DamageType damage_type;
    public AttackAnimationType attack_animation;
    public GameObject projectile_prefab;
    public AnimationCurve shot_power;
    public AnimationCurve shot_delay;
}
