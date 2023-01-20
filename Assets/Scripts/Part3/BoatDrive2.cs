using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BoatDrive2 : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 500.0f;
    public float maxRotationSpeed = 1.0f;

    public Transform arrivalPoint;
    public float distanceThreshold = 1.0f;
    private bool reached = false;

    private Rigidbody rb;

    public GameObject cameraObject;
    //public float maxAngleDeg;
    //float maxAngle;

    Gyroscope gyro;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gyro = Input.gyro;
        gyro.enabled = true;
        //maxAngle = Mathf.PI / 180 * maxAngleDeg;
    }

    void FixedUpdate()
    {
        Move();
        print(rb.velocity);
    }
    
    void Move()
    {
        float x = Input.acceleration.x;
        float z = Input.acceleration.y;

        Vector3 moveDirection = new Vector3(x, 0, z);
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * Mathf.Min(rotationSpeed, maxRotationSpeed));
            transform.rotation = newRotation;
        }
        transform.position += moveDirection * speed * Time.deltaTime;

        // Check distance to arrival point
        float distance = Vector3.Distance(transform.position, arrivalPoint.position);
        if (distance <= distanceThreshold && !reached)
        {
            reached = true;

            //felicitations
            Debug.Log("Bravoo!");

            //Move camera
            cameraObject.SendMessage("goToNext");
            
        }
    }

}
