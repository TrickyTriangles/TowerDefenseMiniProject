using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    [SerializeField] private MeshRenderer redicle;

    // Update is called once per frame
    void Update()
    {
        ShowPosition();


        void ShowPosition()
        {

            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                if (hit.point.z >= 0 && hit.point.z <= Measurements.mapZ-1
                    && hit.point.x >= 0 && hit.point.x <= Measurements.mapX-1)
                {
                    redicle.gameObject.SetActive(true);
                    transform.localPosition = hit.point;
                }
                else { redicle.gameObject.SetActive(false); }
                //Debug.Log(hit.point);
                //HexCell currentCell = grid.GetCell(hit.point); 
            }
        }
    }
}
