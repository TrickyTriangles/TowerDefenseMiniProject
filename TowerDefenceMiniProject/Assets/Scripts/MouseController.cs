using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Debug.Log("working");
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            ShowPosition();
        }

        void ShowPosition()
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                Debug.Log(hit.point);
                if (hit.point.z >= 0 && hit.point.z <= Measurements.mapZ
                    && hit.point.x >= 0 && hit.point.x <= Measurements.mapX)
                {
                    gameObject.SetActive(true);
                    transform.localPosition = hit.point;
                }
                else { gameObject.SetActive(false); }
                //Debug.Log(hit.point);
                //HexCell currentCell = grid.GetCell(hit.point); 
            }
        }
    }
}
