using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{

    [SerializeField] TileData[] neighbors;


    public TileData GetNeighbor(TileDirection direction) //references enum
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(TileDirection direction, TileData tile)
    {
        neighbors[(int)direction] = tile;
        tile.neighbors[(int)direction.Opposite()] = this; // set self as opposite of neighbor
    }

}
