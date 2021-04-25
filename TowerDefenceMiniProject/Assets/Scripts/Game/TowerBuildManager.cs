using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerBuildManager : MonoBehaviour
{
    [SerializeField] private Reticle reticle;
    [SerializeField] private Transform tower_spawn_menu;

    private Action<TileData> InitiateTowerSpawnEvent;
    private Action<Tower> InitiateTowerLevelUpEvent;
    private Action<TileData> ShowTowerData;
    private Action HideTowerData;

    private void Update()
    {
        TileData tile = reticle.CurrentTile;

        if (Input.GetMouseButtonDown(1))
        {
            if (!tile.HasTower)
            {
                InitiateTowerSpawnEvent?.Invoke(tile);
            }
            else
            {
                InitiateTowerLevelUpEvent?.Invoke(tile.GetTower());
            }
        }

        if (tile != null && tile.HasTower)
        {
            ShowTowerData?.Invoke(tile);
        }
        else
        {
            HideTowerData?.Invoke();
        }
    }

    public void SubscribeToInitiateTowerSpawnEvent(Action<TileData> sub)
    {
        InitiateTowerSpawnEvent += sub;
    }

    public void UnsubscribeToInitiateTowerSpawnEvent(Action<TileData> sub)
    {
        InitiateTowerSpawnEvent -= sub;
    }

    public void SubscribeToInitiateTowerLevelUpEvent(Action<Tower> sub)
    {
        InitiateTowerLevelUpEvent += sub;
    }

    public void UnsubscribeToInitiateTowerLevelUpEvent(Action<Tower> sub)
    {
        InitiateTowerLevelUpEvent -= sub;
    }

    public void SubscribeShowTowerData(Action<TileData> sub)
    {
        ShowTowerData += sub;
    }

    public void UnsubscribeShowTowerData(Action<TileData> sub)
    {
        ShowTowerData -= sub;
    }

    public void SubscribeHideTowerData(Action sub)
    {
        HideTowerData += sub;
    }

    public void UnsubscribeHideTowerData(Action sub)
    {
        HideTowerData -= sub;
    }
}
