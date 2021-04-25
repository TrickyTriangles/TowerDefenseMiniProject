using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple object mover for testing tower ranges.
/// Uses Input class axis variables to move an object in the XZ plane.
/// </summary>
public class ObjectMover : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    void Update()
    {
        Vector3 direction = new Vector3();

        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");

        transform.Translate(direction * speed * Time.deltaTime);
    }
}
