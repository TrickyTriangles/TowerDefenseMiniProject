using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An extremely basic implementation of a tower defence tower.
/// I only coded this to check how to handle targeting 
/// </summary>
public class TowerBehaviorTest : MonoBehaviour
{
    private Transform target;
    private float shot_delay = 1f;
    private bool is_active = true;

    private void Start()
    {
        // All we need to do at start time is enable the tower's active routine.
        StartCoroutine(TowerRoutine());
    }

    private void Update()
    {
        // Get a readout of our current target, if we have one.
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (target != null)
            {
                Debug.Log("Current target: " + target.gameObject.name + ".");
            }
        }

        // Disable the tower.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            is_active = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // There should be an additional check that the object that has entered our range either
        // has a targetable tag or implements some ITargetable interface class.
        // This applies to the other OnTrigger events as well.
        if (target == null)
        {
            Debug.Log("Acquired target " + other.gameObject.name + ".");
            target = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // If the tower loses its current target but there's another target within range, we'll
        // set that to be our current target.
        if (target == null)
        {
            Debug.Log("Acquired target " + other.gameObject.name + ".");
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If our current target walks out of range we'll remove it as a target.
        if (other.transform == target)
        {
            Debug.Log("Target has left the area.");
            target = null;
        }
    }

    /// <summary>
    /// This routine carries the basic logic for our tower.
    /// It follows a pattern of:
    /// 
    /// Shot Cooldown => Check If Has Target => Fire At Target => Shot Cooldown => ...
    /// 
    /// The logic for determining our target is put outside of the coroutine as
    /// it is run asynchronously and can cause some pretty major errors if we leave it in.
    /// If the object we're tracking were to be destroyed between steps, then we'd start
    /// firing at an object that no longer exists.
    /// </summary>
    private IEnumerator TowerRoutine()
    {
        float timer = shot_delay;

        while (is_active)
        {
            timer = Mathf.Clamp(timer - Time.deltaTime, 0f, shot_delay);

            if (timer == 0f && target != null)
            {
                timer += shot_delay;
                Debug.Log("Firing at target " + target.gameObject.name + "!");
            }

            yield return null;
        }

        Debug.Log("Tower has deactivated.");
    }
}
