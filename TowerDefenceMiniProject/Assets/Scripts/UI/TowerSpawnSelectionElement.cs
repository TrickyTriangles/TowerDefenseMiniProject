using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawnSelectionElement : MonoBehaviour
{
    [SerializeField] private GameObject object_to_spawn;
    [SerializeField] private string description;
    [SerializeField] TowerSpawnManager tower_spawn_manager;
    [HideInInspector] public bool targeted;
    private float distance = 0f;
    private float target_distance = 30f;

    public void ResetPosition()
    {
        if (tower_spawn_manager != null)
        {
            transform.position = tower_spawn_manager.transform.position;
        }
        else
        {
            transform.position = Vector3.zero;
        }

        distance = 0f;
    }

    private void Update()
    {
        if (targeted)
        {
            distance = Mathf.Lerp(distance, target_distance, 0.3f);
        }
        else
        {
            distance = Mathf.Lerp(distance, 0f, 0.3f);
        }

        Vector3 angles = transform.rotation.eulerAngles;

        transform.position = tower_spawn_manager.transform.position + new Vector3(Mathf.Cos(angles.z * Mathf.Deg2Rad) * distance, Mathf.Sin(angles.z * Mathf.Deg2Rad) * distance, 0f);
    }

    public string GetDescription()
    {
        return description;
    }

    public GameObject GetSelectionObject()
    {
        return object_to_spawn;
    }
}
