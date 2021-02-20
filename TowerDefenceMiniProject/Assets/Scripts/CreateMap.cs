using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public Transform tile;

    private void Start()
    {
        InstantiateMap();
    }

    void InstantiateMap()
    {
        int makeX = Measurements.mapX;
        int makeZ = Measurements.mapZ;

        for (int ix=0; ix< makeX; ix++)
        {
            for(int iz = 0; iz< makeZ; iz++)
            {
                Transform instance = Instantiate(tile);
                instance.localPosition = new Vector3(ix,0,iz);
            }
        }
    }
}
