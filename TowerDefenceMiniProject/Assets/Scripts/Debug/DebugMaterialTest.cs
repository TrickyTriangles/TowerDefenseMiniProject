using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMaterialTest : MonoBehaviour
{
    [SerializeField] private MeshRenderer mr;
    [SerializeField] private Color new_color;

    private void Start()
    {
        if (mr != null)
        {
            mr.materials[0].color = new_color;
        }
    }
}
