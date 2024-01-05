using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAt : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        // Get the forward direction from the camera's rotation, excluding the y-axis rotation
        Vector3 forwardWithoutY = Camera.main.transform.rotation * Vector3.forward;
        forwardWithoutY.y = 0f; // Set y-component to zero to exclude y-axis rotation

        // Calculate the new look-at rotation
        Quaternion lookRotation = Quaternion.LookRotation(forwardWithoutY);

        // Apply the rotation to the UI element
        transform.rotation = lookRotation;
    }
}
