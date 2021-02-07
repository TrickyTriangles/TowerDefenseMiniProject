using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToDestroy : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log(gameObject.name + " has been destroyed.");
        Destroy(gameObject);
    }
}
