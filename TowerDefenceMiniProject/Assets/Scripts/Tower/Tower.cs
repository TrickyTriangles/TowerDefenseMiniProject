using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerProfile tower_profile;
    [SerializeField] private CapsuleCollider range_collider;
    [SerializeField] private Transform action_point;
    [SerializeField] private Transform firing_object_transform;
    [SerializeField] private Animator firing_object_animator;
    [SerializeField] private ParticleSystem firing_particle_system;

    private Transform target;
    [SerializeField] [Range(1, 10)] private int level;
    private float current_shot_delay;
    private bool is_active = true;

    private void Entity_OnDeath(object sender, EventArgs e)
    {
        target = null;
    }

    private void Start()
    {
        InitializeTowerSO();
        StartCoroutine(TowerRoutine());
    }

    private void InitializeTowerSO()
    {
        if (tower_profile != null)
        {
            current_shot_delay = tower_profile.shot_delay.Evaluate(level);

            if (range_collider != null)
            {
                range_collider.radius = tower_profile.tower_range.Evaluate(level);
            }
        }
    }

    private void Update()
    {
        if (target != null)
        {
            UpdateFiringObjectRotation();
        }
    }

    private void UpdateFiringObjectRotation()
    {
        Vector3 direction_to_target = target.position - transform.position;
        direction_to_target.y = 0f;

        Quaternion look_at = Quaternion.LookRotation(direction_to_target.normalized, Vector3.up);
        firing_object_transform.rotation = look_at;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target == null && other.gameObject.CompareTag("Targetable"))
        {
            target = other.transform;
            StartCoroutine(TrackTargetRoutine());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (target == null && other.gameObject.CompareTag("Targetable"))
        {
            target = other.transform;
            StartCoroutine(TrackTargetRoutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == target)
        {
            target = null;
        }
    }

    public void LoseTarget()
    {
        target = null;
    }

    public void LevelUp()
    {
        level++;
        current_shot_delay = tower_profile.shot_delay.Evaluate(level);

        if (range_collider != null)
        {
            range_collider.radius = tower_profile.tower_range.Evaluate(level);
        }
    }

    private DamageProfile GetDamageProfile()
    {
        DamageProfile new_profile = new DamageProfile();

        new_profile.fired_by = gameObject;
        new_profile.damage_type = tower_profile.damage_type;
        new_profile.base_damage = (int)tower_profile.shot_power.Evaluate(level);

        return new_profile;
    }

    private void ShootProjectile()
    {
        TowerProjectile new_projectile = Instantiate(tower_profile.projectile_prefab, action_point.position, Quaternion.identity).GetComponent<TowerProjectile>();

        new_projectile.SetTarget(target);
        new_projectile.SetDamageProfile(GetDamageProfile());
    }

    private IEnumerator TowerRoutine()
    {
        float timer = current_shot_delay;

        while (is_active)
        {
            timer = Mathf.Clamp(timer - Time.deltaTime, 0f, current_shot_delay);

            if (timer == 0f && target != null)
            {
                ShootProjectile();
                if (firing_object_animator != null) { firing_object_animator.Play("Fire", -1, 0f); }
                if (firing_particle_system != null) { firing_particle_system.Play(); }

                timer += tower_profile.shot_delay.Evaluate(level);
            }

            yield return null;
        }

        Debug.Log(name + " has deactivated.");
    }

    private IEnumerator TrackTargetRoutine()
    {
        Entity entity = target.gameObject.GetComponent<Entity>();
        entity.OnDeath += Entity_OnDeath;

        while (target != null)
        {
            yield return null;
        }

        entity.OnDeath -= Entity_OnDeath;
    }
}

[CreateAssetMenu(menuName = "New Tower Profile")]
public class TowerProfile : ScriptableObject
{
    public DamageProfile.DamageType damage_type;
    public GameObject projectile_prefab;
    public AnimationCurve tower_range;
    public AnimationCurve shot_power;
    public AnimationCurve shot_delay;
}
