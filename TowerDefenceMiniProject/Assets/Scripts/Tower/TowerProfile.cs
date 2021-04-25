using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Tower Profile")]
public class TowerProfile : ScriptableObject
{
    public DamageProfile.DamageType damage_type;
    public GameObject projectile_prefab;
    public AnimationCurve upgrade_cost;
    public AnimationCurve tower_range;
    public AnimationCurve shot_power;
    public AnimationCurve shot_delay;
}
