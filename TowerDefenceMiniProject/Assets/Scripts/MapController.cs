using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    public Transform tile;
    public Transform master;
    public List <TileData> playGrid;

    int searchFrontierPhase;

    TileDataPriorityQueue searchFrontier;

    TileData[] tiles;

    public bool HasPath { get { return currentPathExists; } }

    int tileLayer;
        

    private void Start()
    {
        tileLayer = LayerMask.GetMask("TILES");
       
        playGrid = new List<TileData>();
        DetectMap();
        //InstantiateMap();
    }

    #region Map Methods
    void DetectMap()
    {
        int makeX = Measurements.mapX;
        int makeZ = Measurements.mapZ;
        
        //tiles = new TileData[transform.childCount];
        int i = 0;
        //Debug.Log(transform.childCount);
        for (int ix = 0; ix < makeX; ix++)
        {
            for (int iz = 0; iz < makeZ; iz++)
            {
                Ray ray = new Ray(new Vector3(ix+0.5f,5,iz+0.5f),Vector3.down);
                RaycastHit hit;
                if(Physics.Raycast(ray,out hit,2000f,tileLayer))
                {
                    TileData foundTile = hit.collider.gameObject.GetComponent<TileData>();

                    AddTile(ix, iz, i++, foundTile);
                }
                //CreateTile(ix, iz, i++);

            }
        }
    }

    public TileData DetectTile(GameObject source)
    {
        Ray ray = new Ray(new Vector3(source.transform.position.x, 5, source.transform.position.z), Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2000f, tileLayer))
        {
            TileData foundTile = hit.collider.gameObject.GetComponent<TileData>();
            Debug.Log(foundTile);
            return foundTile;
        }
        return null;
    }

    void AddTile(int x, int z, int i, TileData newTile)
    {

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


    #endregion

    #region Path Finding


    TileData currentPathFrom, currentPathTo;
    bool currentPathExists;

    public List<TileData> GetPath()
    {
        if (!currentPathExists)
        {
            return null;
        }
        List<TileData> path = new List<TileData>();
        for (TileData c = currentPathTo; c != currentPathFrom; c = c.PathFrom)
        {
            path.Add(c);
        }
        path.Add(currentPathFrom);
        path.Reverse();
        return path;
    }

    public void FindPath(TileData fromCell, TileData toCell)
    {
        
        ClearPath();
        currentPathFrom = fromCell;
        currentPathTo = toCell;
        currentPathExists = Search(fromCell, toCell);
    }

    public void ClearPath()
    {
        if (currentPathExists)
        {
            TileData current = currentPathTo;
            while (current != currentPathFrom)
            {

                current = current.PathFrom;
            }
            currentPathExists = false;
        }

        currentPathFrom = currentPathTo = null;
    }


    bool Search(TileData fromCell, TileData toCell)
    {
        searchFrontierPhase += 2;

        if (searchFrontier == null)
        {
            searchFrontier = new TileDataPriorityQueue();
        }
        else
        {
            searchFrontier.Clear();
        }

        fromCell.SearchPhase = searchFrontierPhase;
        fromCell.Distance = 0;
        searchFrontier.Enqueue(fromCell);

        while (searchFrontier.Count > 0)
        {
            //yield return delay;
            TileData current = searchFrontier.Dequeue();
            current.SearchPhase += 1;

            if (current == toCell)
            {
                return true;
            }

            // turns for travel time
            //int currentTurn = (current.Distance - 1) / speed;

            for (TileDirection d = TileDirection.N; d <= TileDirection.W; d++)
            {
                TileData neighbor = current.GetNeighbor(d);

                if (neighbor == null || neighbor.SearchPhase > searchFrontierPhase) { continue; }
                if(current.gameObject.tag == "Barrier") { continue; }

                int moveCost;
                if (current.gameObject.tag == "Road") { moveCost = 3; }
                else { moveCost = 5; }


                int distance = current.Distance + moveCost; // remove later, switch to subtract
 

                //if (neighbor.Distance == int.MaxValue)
                if (neighbor.SearchPhase < searchFrontierPhase)
                {
                    neighbor.SearchPhase = searchFrontierPhase;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    // change to cell difference abs(x1-x2) + abs(z1-z2)
                    neighbor.SearchHeuristic = (int) Mathf.Abs(
                            (neighbor.transform.position.x - transform.position.x)
                            + (neighbor.transform.position.z - transform.position.z));
                        //neighbor.coordinates.DistanceTo(toCell.coordinates);
                    searchFrontier.Enqueue(neighbor);
                }
                else if (distance < neighbor.Distance)
                {
                    int oldPriority = neighbor.SearchPriority;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    searchFrontier.Change(neighbor, oldPriority);
                }
            }
        }
        return false;
    }

    #endregion

}
