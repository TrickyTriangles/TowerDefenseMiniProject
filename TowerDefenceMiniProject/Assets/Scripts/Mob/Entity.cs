using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    public event EventHandler OnDeath;

    public void InvokeOnDeath()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }
}
