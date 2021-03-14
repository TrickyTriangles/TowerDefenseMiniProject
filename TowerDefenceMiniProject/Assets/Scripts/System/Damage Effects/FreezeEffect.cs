using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEffect : DamageEffect
{
    private Mob mob;
    private Vector3 offset = new Vector3(0f, 0.5f, 0f);

    public FreezeEffect(Entity _target, float _duration)
    {
        target = _target;
        duration = _duration;

        mob = target.GetComponent<Mob>();
    }

    public override void BeginEffect()
    {
        if (EnvironmentText.IsInitialized)
        {
            EnvironmentText.Instance.DrawText("Freeze!", EnvironmentText.TextTypes.FREEZE, target.transform.position + offset);
        }
    }

    public override void ProcessEffect()
    {
        mob._velocity = 0f;
        mob._animation_playback_speed = 0f;
    }

    public override void EndEffect()
    {
        mob._velocity = mob._base_velocity;
        mob._animation_playback_speed = 1f;
    }
}
