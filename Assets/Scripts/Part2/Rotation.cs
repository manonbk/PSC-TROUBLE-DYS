/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour{
   private Vector2[] touchStartPos = new Vector2[2];
    private float initialRotation;

    // Adjust this speed based on your preference
    public float rotationSpeed = 2f;

    void Update()
    {
        // Check if there are two touches
        if (Input.touchCount == 2)
        {
            // Get the first touch
            Touch touch1 = Input.GetTouch(0);

            // Get the second touch
            Touch touch2 = Input.GetTouch(1);

            // Check if fingers are on screen
            //if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began){
                // Check if both touches are on the object
                //if (Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1))
                if(touch1.phase ==TouchPhase.Began || touch2.phase ==TouchPhase.Began)
                {
                    // Store the initial touch positions and the initial rotation of the object
                    touchStartPos[0] = touch1.position;
                    touchStartPos[1] = touch2.position;
                    initialRotation = transform.eulerAngles.z;

                    // Check if both touches moved
                    if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
                    {
                        // Calculate the current touch positions
                        Vector2 touchCurrentPos1 = touch1.position;
                        Vector2 touchCurrentPos2 = touch2.position;

                        // Calculate the initial vector from the first touch
                        Vector2 initialVector = touchStartPos[1] - touchStartPos[0];

                        // Calculate the current vector
                        Vector2 currentVector = touchCurrentPos2 - touchCurrentPos1;

                        // Calculate the rotation angle based on the signed angle between initial and current vectors
                        float rotationAngle = Vector2.SignedAngle(initialVector, currentVector);

                        // Apply rotation to the object around the forward (Z) axis
                        transform.rotation = Quaternion.Euler(0, 0, initialRotation + rotationAngle * rotationSpeed);
                    }
                }
            //}
        }
    }
        
    // Check if the touch position is on the object
    /*private bool IsTouchOnObject(Vector2 touchPosition)
    {
        Collider2D collider = GetComponent<Collider2D>();

        if (collider != null)
        {
            return collider.bounds.Contains(Camera.main.ScreenToWorldPoint(touchPosition));
        }

        return false;
    }*/
/*}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    private bool isRotating = false;
    private Vector2 initialTouchPosition;
    private float rotationSpeed = 5f;

    void Update()
    {
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        // Check if there are at least two touches
        if (Input.touchCount >= 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            switch (touch1.phase)
            {
                case TouchPhase.Began:
                    if (IsTouchOnObject(touch1.position))
                    {
                        isRotating = true;
                        initialTouchPosition = touch2.position - touch1.position;
                    }
                    break;

                case TouchPhase.Moved:
                    if (isRotating)
                    {
                        Vector2 currentTouchPosition = touch2.position - touch1.position;
                        float rotationAmount = Vector2.SignedAngle(initialTouchPosition, currentTouchPosition);
                        RotateObject(rotationAmount * rotationSpeed * Time.deltaTime);
                        initialTouchPosition = currentTouchPosition;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isRotating = false;
                    break;
            }
        }
    }

    bool IsTouchOnObject(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject == gameObject;
        }

        return false;
    }

    void RotateObject(float rotationAmount)
    {
        // Rotate the object around its up axis (perpendicular to the plane containing the object)
        transform.Rotate(Vector3.up, rotationAmount, Space.World);
    }
}
