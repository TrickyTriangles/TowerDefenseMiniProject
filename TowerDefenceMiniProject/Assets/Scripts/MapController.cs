using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public Transform tile;
    public Transform master;
    public List <TileData> playGrid;

    TileData[] tiles;

    private void Start()
    {
        
        playGrid = new List<TileData>();
        InstantiateMap();
    }


    //***************************************//

    void InstantiateMap()
    {
        int makeX = Measurements.mapX;
        int makeZ = Measurements.mapZ;
        tiles = new TileData[makeX * makeZ];
        int i = 0;

        for (int ix=0; ix< makeX; ix++)
        {
            for(int iz = 0; iz< makeZ; iz++)
            {
                CreateTile(ix, iz, i++);

            }
        }
    }

    void CreateTile (int x, int z, int i)
    {
        TileData newTile = tiles[i] = Instantiate(tile).GetComponent<TileData>();
        newTile.transform.localPosition = new Vector3(x, 0, z);
        newTile.transform.parent = master;

        playGrid.Add(newTile);


        if (x > 0)
        {
            TileData lastTile = playGrid[i - Measurements.mapZ];
            newTile.SetNeighbor(TileDirection.W, lastTile);
        }

        if (z > 0)
        {
            TileData oppositeTile = playGrid[i - 1];
            newTile.SetNeighbor(TileDirection.S, oppositeTile);
        }
    }
}
