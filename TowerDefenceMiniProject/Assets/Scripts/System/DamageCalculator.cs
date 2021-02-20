using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageCalculator
{
    public static int CalculateDamage(DamageProfile attacker, MobProfile mob)
    {
        float running_total = attacker.base_damage;

        switch (attacker.damage_type)
        {
            case DamageProfile.DamageType.NORMAL:
                running_total *= 1 - mob.normal_damage_resistance;
                break;
            case DamageProfile.DamageType.SLOW:
                running_total *= 1 - mob.slow_damage_resistance;
                break;
            case DamageProfile.DamageType.FREEZE:
                running_total *= 1 - mob.freeze_damage_resistance;
                break;
            case DamageProfile.DamageType.BURN:
                running_total *= 1 - mob.burn_damage_resistance;
                break;
            default:
                break;
        }

        return Mathf.CeilToInt(running_total);
    }

    public static int CalculateDamage(DamageProfile attacker, HeroProfile hero)
    {
        return attacker.base_damage;
    }
}
