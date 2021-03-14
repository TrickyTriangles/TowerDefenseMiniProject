using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnEffect : DamageEffect
{
    private Mob mob;
    private DamageProfile attack_info;
    private float burn_timer = 0f;
    private float burn_tick;
    private float burn_damage;

    private Vector3 offset = new Vector3(0f, 0.5f, 0f);

    public BurnEffect(Entity _target, DamageProfile _attack_info, float _duration, float _burn_tick, float _burn_damage)
    {
        target = _target;
        attack_info = _attack_info;
        duration = _duration;
        burn_tick = _burn_tick;
        burn_damage = _burn_damage;

        mob = target.GetComponent<Mob>();
    }

    public override void BeginEffect()
    {
        if (EnvironmentText.IsInitialized)
        {
            EnvironmentText.Instance.DrawText("Burn!", EnvironmentText.TextTypes.BURN, target.transform.position + offset);
        }
    }

    public override void ProcessEffect()
    {
        burn_timer += Time.deltaTime;

        if (burn_timer >= burn_tick)
        {
            burn_timer -= burn_tick;
            mob.ReduceHealth(Mathf.CeilToInt(burn_damage), attack_info);

            if (EnvironmentText.IsInitialized)
            {
                EnvironmentText.Instance.DrawText(Mathf.CeilToInt(burn_damage).ToString(), EnvironmentText.TextTypes.BURN, target.transform.position);
            }
        }
    }

    public override void EndEffect()
    {
    }
}
