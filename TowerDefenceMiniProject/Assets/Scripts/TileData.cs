using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    [SerializeField] TileData[] neighbors;
    public TileData PathFrom { get; set; }
    public int SearchHeuristic { get; set; }

    public TileData NextWithSamePriority { get; set; }

    public int SearchPhase { get; set; }

    int distance;
    public int Distance { get { return distance; } set { distance = value; } }
    public int SearchPriority { get { return distance + SearchHeuristic; } }

    private Tower tower;
    public bool HasTower
    {
        get { return tower != null; }
    }

    //public HexUnit Unit { get; set; }

    public TileData GetNeighbor(TileDirection direction) //references enum
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(TileDirection direction, TileData tile)
    {
        neighbors[(int)direction] = tile;
        tile.neighbors[(int)direction.Opposite()] = this; // set self as opposite of neighbor
    }

    public void SetTower(GameObject new_tower)
    {
        if (HasTower)
        {
            Destroy(tower.gameObject);
        }

        tower = Instantiate(new_tower, transform.position + new Vector3(0f, 0.2f), Quaternion.identity).GetComponent<Tower>();
    }

    public Tower GetTower()
    {
        return tower;
    }
}
