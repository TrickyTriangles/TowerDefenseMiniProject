using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : DamageEffect
{
    private float velocity_dampening;
    private Mob mob;
    private Vector3 offset = new Vector3(0f, 0.5f, 0f);

    public SlowEffect(Entity _target, float _duration, float _dampening)
    {
        target = _target;
        duration = _duration;
        velocity_dampening = _dampening;
    }

    public override void BeginEffect()
    {
        if (EnvironmentText.IsInitialized)
        {
            EnvironmentText.Instance.DrawText("Slow!", EnvironmentText.TextTypes.SLOW, target.transform.position + offset);
        }

        mob = target.GetComponent<Mob>();
    }

    public override void ProcessEffect()
    {
        if (mob != null)
        {
            float new_velocity = mob._base_velocity * velocity_dampening;

            if (new_velocity < mob._velocity)
            {
                mob._velocity = new_velocity;
                mob._animation_playback_speed = velocity_dampening;
            }
        }
    }

    public override void EndEffect()
    {
        if (mob != null)
        {
            mob._velocity = mob._base_velocity;
            mob._animation_playback_speed = 1f;
        }
    }
}
