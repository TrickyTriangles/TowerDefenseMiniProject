using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject skeleton_prefab;
    [SerializeField] private GameObject bomb_prefab;
    [SerializeField] private GameObject coin_prefab;
    private Vector3 spawn_point = new Vector3(0f, 0.25f, 0f);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (GameManager.IsInitialized)
            {
                Entity entity = Instantiate(bomb_prefab, spawn_point, Quaternion.identity).GetComponent<Entity>();
                entity.OnDeath += Entity_OnDeath;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (GameManager.IsInitialized)
            {
                Entity entity = Instantiate(skeleton_prefab, spawn_point, Quaternion.identity).GetComponent<Entity>();
                entity.OnDeath += Entity_OnDeath;
            }
        }
    }

    private void Entity_OnDeath(object sender, EventArgs args)
    {
        var entity = sender as Mob;

        if (entity != null)
        {
            Coin coin = Instantiate(coin_prefab, entity.transform.position, Quaternion.identity).GetComponent<Coin>();

            if (coin != null) { coin.Subscribe_OnCollected(GameManager.Instance.CoinCollectDelegate); }
        }
    }
}
