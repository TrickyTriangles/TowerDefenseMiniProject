using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSpawner : MonoBehaviour
{
    [SerializeField] private GameObject skeleton;
    [SerializeField] private GameObject target;
    [SerializeField] private MapController master;
    [SerializeField] [Range(0.1f, 3f)] float delay;
    private bool is_active;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            is_active = !is_active;

            if (is_active)
            {
                StartCoroutine(SpawnRoutine());
            }
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (is_active)
        {
            PathfindingMovement movement = Instantiate(skeleton, transform.position, Quaternion.identity).GetComponent<PathfindingMovement>();
            movement.SetTarget(target);
            movement.master = master;

            yield return new WaitForSeconds(delay);
        }
    }
}
