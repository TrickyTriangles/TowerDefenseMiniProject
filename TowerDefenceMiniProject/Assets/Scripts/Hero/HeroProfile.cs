using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
