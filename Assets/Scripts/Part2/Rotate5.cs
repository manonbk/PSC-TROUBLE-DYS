using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate5 : MonoBehaviour
{
    private Vector2 touchStartPos;
    private float initialRotation;

    // Adjust this speed based on your preference
    public float rotationSpeed = 5f;

    void Update()
    {
        // Check if there are two touches
        if (Input.touchCount == 2)
        {
            // Get the first touch
            Touch touch1 = Input.GetTouch(0);

            // Get the second touch
            Touch touch2 = Input.GetTouch(1);

            // Check if either touch started this frame
            if (touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began)
            {
                // Store the initial touch positions and the initial rotation of the object
                touchStartPos = (touch1.position + touch2.position) / 2f;
                initialRotation = transform.eulerAngles.z;
            }

            // Check if both touches moved
            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                // Calculate the current touch positions
                Vector2 touchCurrentPos = (touch1.position + touch2.position) / 2f;

                // Calculate the delta position from the initial touch positions
                float delta = Vector2.Angle(touchStartPos, touchCurrentPos);

                Vector3 cross = Vector3.Cross(touchStartPos, touchCurrentPos);
                if(cross.z > 0) {
                    delta = -delta;
                }
 

                // Calculate the angle based on the delta position and rotation speed
                float angle = delta * rotationSpeed;

                // Apply rotation to the object around the Z-axis
                transform.rotation = Quaternion.Euler(0, 0, initialRotation + angle);
                transform.Rotate(Vector3.forward, angle);
            }
        }
    }
}
