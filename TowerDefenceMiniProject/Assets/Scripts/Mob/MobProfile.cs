using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Mob Profile")]
public class MobProfile : ScriptableObject
{
    public DamageProfile.DamageType damage_type;
    public int base_health;
    public AnimationCurve base_power;
    public int experience;
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