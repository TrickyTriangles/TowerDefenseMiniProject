using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            CheckTowerHit();
    }

    private void CheckTowerHit()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, float.MaxValue, int.MaxValue, QueryTriggerInteraction.Ignore))
        {
            GameObject object_hit = hit.transform.gameObject;
            Tower tower = object_hit.GetComponent<Tower>();

            if (tower != null)
            {
                tower.LevelUp();
            }
        }
    }
}
