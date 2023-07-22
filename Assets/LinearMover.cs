using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float targetPositionZ = -6000f;

    private bool isMoving = true;

    private void Update()
    {
        if (isMoving)
        {
            // Calculate the movement distance for this frame
            float step = moveSpeed * Time.deltaTime;

            // Move the object towards the target position
            transform.Translate(Vector3.forward * step);

            // Check if the object has reached the target position
            if (transform.position.z <= targetPositionZ)
            {
                // Stop moving the object
                isMoving = false;
                // Snap the object to the exact target position
                transform.position = new Vector3(transform.position.x, transform.position.y, targetPositionZ);
            }
        }
    }
}
