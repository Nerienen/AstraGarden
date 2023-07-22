using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    public float rotationSpeed = 30f; // The speed of rotation in degrees per second

    void Update()
    {
        // Rotate the object around the Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
