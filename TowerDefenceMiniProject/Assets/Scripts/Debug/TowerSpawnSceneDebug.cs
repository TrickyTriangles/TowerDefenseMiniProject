using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawnSceneDebug : MonoBehaviour
{
    [SerializeField] private GameManager game_manager;
    [SerializeField] private GameObject skeleton;

    private void Update()
    {
        if (game_manager != null)
        {
            if (Input.GetKeyDown(KeyCode.Equals))
            {
                game_manager.AwardGold(10);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                game_manager.AwardGold(10000);
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (skeleton != null)
            {
                skeleton.SetActive(true);
            }
        }
    }
}
