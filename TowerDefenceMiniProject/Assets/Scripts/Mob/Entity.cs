using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    public class OnDeathEventArgs : EventArgs
    {
        public DamageProfile last_hit;
    }

    public event EventHandler OnDeath;

    public void InvokeOnDeath(EventArgs args)
    {
        OnDeath?.Invoke(this, args);
    }
}
