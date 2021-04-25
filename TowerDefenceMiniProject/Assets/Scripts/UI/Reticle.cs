using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Reticle : MonoBehaviour
{
    [SerializeField] private GameObject reticle;
    [SerializeField] private MeshRenderer reticle_renderer;
    [SerializeField] private Camera main_camera;
    private bool locked;
    private LayerMask mask;
    private TileData current_tile;
    public TileData CurrentTile
    {
        get { return current_tile; }
    }

    private void Start()
    {
        mask = LayerMask.GetMask("TILES");
    }

    private void Update()
    {
        if (!locked)
        {
            if (reticle != null && main_camera != null)
            {
                Ray ray = main_camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, float.MaxValue, mask))
                {
                    reticle.SetActive(true);
                    TileData tile_data = hit.collider.gameObject.GetComponent<TileData>();

                    if (tile_data != null)
                    {
                        transform.position = tile_data.transform.position;
                        current_tile = tile_data;
                    }
                }
                else
                {
                    current_tile = null;
                    reticle.SetActive(false);
                }
            }
        }
    }

    public void LockReticle()
    {
        locked = true;
    }

    public void UnlockReticle()
    {
        locked = false;
    }
}
