using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerSpawnManager : MonoBehaviour
{
    [SerializeField] private TowerBuildManager build_manager;
    [SerializeField] private Camera _camera;
    [SerializeField] private Reticle reticle;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image money_icon;
    [SerializeField] private TextMeshProUGUI money_readout;
    [SerializeField] private TowerSpawnSelectionElement[] selection_elements;
    [SerializeField] private Vector2[] selection_ranges;
    [SerializeField] private float deadzone;
    private Coroutine spawn_routine;

    private void Start()
    {
        if (build_manager != null)
        {
            build_manager.SubscribeToInitiateTowerSpawnEvent(TowerBuildManager_InitiateTowerSpawn);
        }

        for (int i = 0; i < selection_elements.Length; i++)
        {
            selection_elements[i].gameObject.SetActive(false);
        }

        if (description != null) { description.gameObject.SetActive(false); }
    }

    private void TowerBuildManager_InitiateTowerSpawn(TileData tile)
    {
        if (spawn_routine == null)
        {
            spawn_routine = StartCoroutine(SpawnTowerRoutine(tile));
        }
    }

    private float GetMouseAngle()
    {
        float result = Mathf.Atan2(Input.mousePosition.y - transform.position.y, Input.mousePosition.x - transform.position.x);

        return result * Mathf.Rad2Deg;
    }

    private IEnumerator SpawnTowerRoutine(TileData tile)
    {
        if (reticle != null) { reticle.LockReticle(); }
        if (description != null) { description.gameObject.SetActive(true); }

        for (int i = 0; i < selection_elements.Length; i++)
        {
            selection_elements[i].gameObject.SetActive(true);
            selection_elements[i].ResetPosition();
        }

        transform.position = Input.mousePosition;
        int selection = -1;

        while (Input.GetMouseButton(1))
        {
            float dist = Vector3.Distance(transform.position, Input.mousePosition);

            float angle = GetMouseAngle();
            selection = -1;

            for (int i = 0; i < selection_elements.Length; i++)
            {
                if (i == 0) // This is our seam case, where the angle returned by Atan2 switches from 180 to -179.9999
                {
                    if (angle >= selection_ranges[i].x || angle < selection_ranges[i].y)
                    {
                        selection_elements[i].targeted = (dist > deadzone);
                        selection = i;
                    }
                    else
                    {
                        selection_elements[i].targeted = false;
                    }
                }
                else
                {
                    if (angle >= selection_ranges[i].x && angle < selection_ranges[i].y)
                    {
                        selection_elements[i].targeted = (dist > deadzone);
                        selection = i;
                    }
                    else
                    {
                        selection_elements[i].targeted = false;
                    }
                }
            }

            if (selection > -1 && selection_elements[selection].targeted)
            {
                description.text = selection_elements[selection].GetDescription();
                description.color = GameManager.Instance.Gold >= selection_elements[selection].GetBuildCost() ? Color.white : Color.red;
                money_icon.gameObject.SetActive(true);
                money_readout.text = selection_elements[selection].GetBuildCost().ToString();
                money_readout.color = GameManager.Instance.Gold >= selection_elements[selection].GetBuildCost() ? Color.white : Color.red;
            }
            else
            {
                description.text = "";
                money_icon.gameObject.SetActive(false);
                money_readout.text = "";
            }

            yield return null;
        }

        if (selection > -1 && selection_elements[selection].targeted)
        {
            int build_cost = selection_elements[selection].GetBuildCost();

            if (GameManager.Instance.Gold >= build_cost && build_cost > -1)
            {
                tile.SetTower(selection_elements[selection].GetSelectionObject());
                GameManager.Instance.RemoveGold(build_cost);
            }
        }

        if (reticle != null) { reticle.UnlockReticle(); }
        if (description != null) { description.gameObject.SetActive(false); }

        for (int i = 0; i < selection_elements.Length; i++)
        {
            selection_elements[i].gameObject.SetActive(false);
        }

        spawn_routine = null;
    }
}
