using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageCalculator
{
    public struct DamageCalculationResult
    {
        public DamageProfile.DamageType damage_type;
        public int damage_total;
        public List<DamageEffect> damage_effects;
    }

    public static void CalculateDamage(DamageProfile attacker, MobProfile mob_profile, Mob mob, out DamageCalculationResult result)
    {
        result = new DamageCalculationResult();
        result.damage_effects = new List<DamageEffect>();

        float running_total = attacker.damage_modifier == 0 ? attacker.base_damage : attacker.base_damage * attacker.damage_modifier;

        switch (attacker.damage_type)
        {
            case DamageProfile.DamageType.NORMAL:
                running_total *= 1 - mob_profile.normal_damage_resistance;
                result.damage_type = DamageProfile.DamageType.NORMAL;
                break;
            case DamageProfile.DamageType.SLOW:
                running_total *= 1 - mob_profile.slow_damage_resistance;
                result.damage_type = DamageProfile.DamageType.SLOW;

                float slow_roll = Random.Range(0f, 1f);
                if (slow_roll > mob_profile.slow_chance_resist)
                {
                    SlowEffect slow = new SlowEffect(mob, 3f, 0.5f);
                    result.damage_effects.Add(slow);
                }
                break;
            case DamageProfile.DamageType.FREEZE:
                running_total *= 1 - mob_profile.freeze_damage_resistance;
                result.damage_type = DamageProfile.DamageType.FREEZE;

                float freeze_roll = Random.Range(0f, 1f);
                if (freeze_roll > mob_profile.freeze_chance_resist)
                {
                    FreezeEffect freeze = new FreezeEffect(mob, 1f);
                    result.damage_effects.Add(freeze);
                }
                break;
            case DamageProfile.DamageType.BURN:
                running_total *= 1 - mob_profile.burn_damage_resistance;
                result.damage_type = DamageProfile.DamageType.BURN;

                float burn_roll = Random.Range(0f, 1f);
                if (burn_roll > mob_profile.burn_chance_resist)
                {
                    BurnEffect burn = new BurnEffect(mob, attacker, 3f, 0.4f, 2f);
                    result.damage_effects.Add(burn);
                }
                break;
            default:
                break;
        }

        result.damage_total = Mathf.CeilToInt(running_total);
    }

    public static void CalculateDamage(DamageProfile attacker, HeroProfile hero, out DamageCalculationResult result)
    {
        result = new DamageCalculationResult();
    }
}
