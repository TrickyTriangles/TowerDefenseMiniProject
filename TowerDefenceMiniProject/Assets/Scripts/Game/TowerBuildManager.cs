using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerBuildManager : MonoBehaviour
{
    [SerializeField] private Reticle reticle;
    [SerializeField] private Transform tower_spawn_menu;
    private Action<TileData> InitiateTowerSpawnEvent;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TileData tile = reticle.CurrentTile;

            if (!tile.HasTower)
            {
                InitiateTowerSpawnEvent?.Invoke(tile);
            }
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
}
