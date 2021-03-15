using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMob : Mob, IDamageable
{
    [SerializeField] private float fuse_time;
    [SerializeField] private float explosion_radius;
    [SerializeField] private GameObject explosion_prefab;
    [SerializeField] private ParticleSystem fuse_particle;
    private Coroutine fuse_routine;

    void IDamageable.Damage(DamageProfile damage_profile)
    {
        if (damage_profile.damage_type == DamageProfile.DamageType.BURN && fuse_routine == null)
        {
            fuse_routine = StartCoroutine(FuseRoutine());
        }

        HitMob(damage_profile);
    }

    private void CreateExplosion()
    {
        GameObject exp_prefab = Instantiate(explosion_prefab, transform.position, Quaternion.identity);
        Explosion exp = exp_prefab.GetComponent<Explosion>();

        if (exp != null)
        {
            exp.SetInitialValues(GetDamageProfile(), explosion_radius);
        }
    }

    private DamageProfile GetDamageProfile()
    {
        DamageProfile dp = new DamageProfile();

        dp = mob_profile.GetDamageProfile();
        dp.damage_type = DamageProfile.DamageType.NORMAL;
        dp.damage_modifier = 5f;

        return dp;
    }

    private IEnumerator FuseRoutine()
    {
        if (EnvironmentText.IsInitialized)
        {
            EnvironmentText.Instance.DrawText("!!!", EnvironmentText.TextTypes.BURN, transform.position + new Vector3(0f, 0.75f, 0f));
        }

        if (fuse_particle != null)
            fuse_particle.Play();

        yield return new WaitForSeconds(fuse_time);

        if (explosion_prefab != null)
        {
            CreateExplosion();
        }

        KillMob(new DamageProfile());
    }
}
