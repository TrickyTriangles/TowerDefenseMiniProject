using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class Tower : MonoBehaviour
{
    private const int MAX_LEVEL = 4;
    [SerializeField] private TowerProfile tower_profile;
    [SerializeField] private CapsuleCollider range_collider;
    [SerializeField] private Transform action_point;
    [SerializeField] private Transform firing_object_transform;
    [SerializeField] private Animator firing_object_animator;
    [SerializeField] private Animator level_up_effect_animator;
    [SerializeField] private ParticleSystem firing_particle_system;
    [SerializeField] private MeshRenderer[] tower_meshes;
    [SerializeField] private Transform[] cannon_locations;

    private Coroutine level_up_routine;
    private bool _loaded;
    private float current_shot_delay;
    private bool is_active = true;
    private bool is_paused = false;
    private Transform target;

    [SerializeField] [Range(1, MAX_LEVEL)] private int level;
    public int Level
    {
        get { return level; }
        set { level = value; if (_loaded) { AdjustLevelDetails(); } }
    }

    public bool IsMaxLevel
    {
        get { return level == MAX_LEVEL; }
    }

    public int UpgradeCost
    {
        get { return !IsMaxLevel ? (int)tower_profile.upgrade_cost.Evaluate(level + 1) : 0; }
    }

    public int BuildCost
    {
        get { return (int)tower_profile.upgrade_cost.Evaluate(1); }
    }

    public int CurrentPower
    {
        get { return (int)tower_profile.shot_power.Evaluate(level); }
    }

    public int NextPower
    {
        get { return (int)tower_profile.shot_power.Evaluate(level + 1); }
    }

    public float CurrentRange
    {
        get { return tower_profile.tower_range.Evaluate(level); }
    }

    public float NextRange
    {
        get { return tower_profile.tower_range.Evaluate(level + 1); }
    }

    public float CurrentDelay
    {
        get { return tower_profile.shot_delay.Evaluate(level); }
    }

    public float NextDelay
    {
        get { return tower_profile.shot_delay.Evaluate(level + 1); }
    }

    private void Entity_OnDeath(object sender, EventArgs e)
    {
        target = null;
    }

    private void OnValidate()
    {
        if (_loaded)
        {
            Level = level;
        }
    }

    private void Start()
    {
        InitializeTowerSO();
        StartCoroutine(TowerRoutine());

        _loaded = true;
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
        if (level < MAX_LEVEL)
        {
            level = Mathf.Clamp(level + 1, 1, MAX_LEVEL);

            if (level_up_routine != null)
            {
                StopCoroutine(level_up_routine);
            }

            level_up_routine = StartCoroutine(LevelUpRoutine());
        }
    }

    public void AdjustLevelDetails()
    {
        current_shot_delay = tower_profile.shot_delay.Evaluate(level);

        if (range_collider != null)
        {
            range_collider.radius = tower_profile.tower_range.Evaluate(level);
        }

        for (int i = 0; i < 4; i++)
        {
            if (i + 1 == level)
            {
                tower_meshes[i].enabled = true;
                firing_object_transform.position = cannon_locations[i].position;
            }
            else
            {
                tower_meshes[i].enabled = false;
            }
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
            if (!is_paused)
            {
                timer = Mathf.Clamp(timer - Time.deltaTime, 0f, current_shot_delay);

                if (timer == 0f && target != null)
                {
                    ShootProjectile();
                    if (firing_object_animator != null) { firing_object_animator.Play("Fire", -1, 0f); }
                    if (firing_particle_system != null) { firing_particle_system.Play(); }

                    timer += tower_profile.shot_delay.Evaluate(level);
                }
            }

            yield return null;
        }
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

    private IEnumerator LevelUpRoutine()
    {
        is_paused = true;

        if (level_up_effect_animator != null)
        {
            float timer = 0f;

            level_up_effect_animator.Play("LevelUpEffect");
            AnimatorStateInfo info = level_up_effect_animator.GetCurrentAnimatorStateInfo(0);

            while (timer < info.length)
            {
                timer += Time.deltaTime;

                yield return null;
            }
        }
        else
        {
            AdjustLevelDetails();
        }

        is_paused = false;
        level_up_routine = null;
    }
}
