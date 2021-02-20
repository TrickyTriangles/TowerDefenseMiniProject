using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 axis;
    [SerializeField] float rotation_speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis.normalized, rotation_speed * Time.deltaTime);
    }
}
