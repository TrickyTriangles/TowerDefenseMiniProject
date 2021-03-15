using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect
{
    public float duration;
    public Entity target;

    public virtual void BeginEffect()
    {
    }

    public virtual void ProcessEffect()
    {
    }

    public virtual void EndEffect()
    {
    }
}
